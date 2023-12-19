using System.Net.Sockets;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Common;
using Mpms.Mpd.Common;
using Timer = System.Timers.Timer;

namespace Mpms.Mpd.Client;

public class MpdClient : IClient
{
    private readonly ILogger<MpdClient> _logger;
    private readonly Timer _heartBeatTimer;
    private readonly IConnectionAdapter _connectionAdapter;
    private NetworkStream? _stream;

    public MpdClient(ILogger<MpdClient> logger, IConnectionAdapterFactory factory, IOptions<MpdConnectionOptions> configuration)
    {
        _logger = logger;
        _heartBeatTimer = new Timer(configuration.Value.HeartBeatInterval * 1000) { AutoReset = true };
        _heartBeatTimer.Elapsed += OnHeartBeatTimerElapsed;
        _connectionAdapter = factory.GetConnectionAdapter(configuration.Value.Type);
    }

    public string? Name { get; private set; }
    public string? ProtocolVersion { get; private set; }
    public bool IsConnectionEstablished => _connectionAdapter.IsConnected;

    public async Task EstablishConnectionAsync()
    {
        _heartBeatTimer.Stop();

        if (_stream is not null)
        {
            await _stream.DisposeAsync()
                .ConfigureAwait(false);

            _stream = null;
        }

        _stream = (NetworkStream) await _connectionAdapter.CreateStreamAsync()
            .ConfigureAwait(false);

        // retrieve MPD version
        var response = new VersionResponse();

        response.ParseData(_stream.ReadAllData());

        Name = response.Name;
        ProtocolVersion = response.Version;

        _heartBeatTimer.Start();
    }

    public async Task<TResponse> SendRequestAsync<TResponse>(IRequest request)
        where TResponse : IResponse, new()
    {
        if (!_connectionAdapter.IsConnected)
            await EstablishConnectionAsync()
                .ConfigureAwait(false);

        if (_stream is null)
            throw new Exception("The stream cannot be null.");

        try
        {
            var sendBuffer = request.Data;

            _heartBeatTimer.Stop();
            await _stream.WriteAsync(sendBuffer, 0, sendBuffer.Length)
                .ConfigureAwait(false);

            var response = new TResponse();
            response.ParseData(_stream.ReadAllData());

            return response;
        }
        finally
        {
            _heartBeatTimer.Start();
        }
    }

    public async Task CloseConnectionAsync()
    {
        _heartBeatTimer.Stop();

        if (!_connectionAdapter.IsConnected)
            return;

        await _connectionAdapter.DisconnectAsync();
    }

    #region IDisposable, IAsyncDisposable

    public void Dispose()
    {
        _stream?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_stream is not null)
            await _stream.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    #endregion

    private async void OnHeartBeatTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (!_connectionAdapter.IsConnected)
            return;

        await SendRequestAsync<PingResponse>(new PingRequest());
    }
}