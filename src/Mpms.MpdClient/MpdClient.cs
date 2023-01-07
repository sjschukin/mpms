using System.Net.Sockets;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Common;
using Mpms.MpdClient.Commands;
using Mpms.MpdClient.Extensions;
using Timer = System.Timers.Timer;

namespace Mpms.MpdClient;

public class MpdClient : IClient
{
    private readonly ILogger<MpdClient> _logger;
    private readonly IConnectionAdapter _connectionAdapter;
    private readonly Timer _heartBeatTimer;
    private NetworkStream? _stream;

    public MpdClient(ILogger<MpdClient> logger, IOptions<MpdConnectionOptions> configuration, IConnectionAdapter connectionAdapter)
    {
        _logger = logger;
        _connectionAdapter = connectionAdapter;
        _heartBeatTimer = new Timer(configuration.Value.HeartBeatInterval) { AutoReset = true };
        _heartBeatTimer.Elapsed += OnHeartBeatTimerElapsed;
        IsConnectionEstablished = false;
    }

    public string? Name { get; private set; }
    public string? ProtocolVersion { get; private set; }
    public bool IsConnectionEstablished { get; private set; }

    public async Task EstablishConnectionAsync()
    {
        _stream = (NetworkStream) await _connectionAdapter.CreateStreamAsync()
            .ConfigureAwait(false);

        var response = new VersionResponse();
        response.ParseData(_stream.ReadAllData());

        IsConnectionEstablished = true;
    }

    public async Task<TResponse> SendRequestAsync<TResponse>(IRequest request)
        where TResponse : IResponse, new()
    {
        if (!IsConnectionEstablished)
            throw new Exception("Connection not established.");

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
        if (!IsConnectionEstablished)
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
        if (!IsConnectionEstablished)
            return;

        await SendRequestAsync<PingResponse>(new PingRequest());
    }
}