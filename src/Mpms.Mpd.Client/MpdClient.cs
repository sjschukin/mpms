using System.Net.Sockets;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Common;
using Mpms.Mpd.Client.Commands;
using Mpms.Mpd.Client.Extensions;
using Mpms.Mpd.Common;
using Timer = System.Timers.Timer;

namespace Mpms.Mpd.Client;

public class MpdClient : IClient
{
    private readonly ILogger<MpdClient> _logger;
    private readonly Timer _heartBeatTimer;
    private readonly IConnectionAdapter _connectionAdapter;
    private NetworkStream? _stream;

    public MpdClient(ILogger<MpdClient> logger, IServiceProvider serviceProvider, IOptions<MpdConnectionOptions> configuration)
    {
        _logger = logger;
        _heartBeatTimer = new Timer(configuration.Value.HeartBeatInterval * 1000) { AutoReset = true };
        _heartBeatTimer.Elapsed += OnHeartBeatTimerElapsed;
        _connectionAdapter =
            serviceProvider.GetKeyedService<IConnectionAdapter>(configuration.Value.Type)
            ?? throw new InvalidOperationException("Could not find a suitable connection adapter from settings.");
    }

    public string? Name { get; private set; }
    public string? ProtocolVersion { get; private set; }
    public bool IsConnectionEstablished => _connectionAdapter.IsConnected;

    public async Task EstablishConnectionAsync(CancellationToken cancellationToken)
    {
        _heartBeatTimer.Stop();

        if (_stream is not null)
        {
            await _stream.DisposeAsync();
            _stream = null;
        }

        _stream = (NetworkStream) await _connectionAdapter.CreateStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Connection established.");

        // retrieve MPD version
        var response = new VersionResponse(await _stream.ReadAllDataAsync(cancellationToken));
        Name = response.Name;
        ProtocolVersion = response.Version;

        _logger.LogInformation("Name: {Name}", Name);
        _logger.LogInformation("Protocol: {Version}", ProtocolVersion);

        _heartBeatTimer.Start();
    }

    public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
        TRequest request, Func<byte[], TResponse> creator, CancellationToken cancellationToken)
        where TRequest : IRequest
        where TResponse : IResponse
    {
        if (!_connectionAdapter.IsConnected)
            await EstablishConnectionAsync(cancellationToken);

        if (_stream is null)
            throw new Exception("The stream cannot be null.");

        try
        {
            byte[] sendBuffer = request.Data;

            _heartBeatTimer.Stop();
            await _stream.WriteAsync(sendBuffer, cancellationToken);

            var response = creator(await _stream.ReadAllDataAsync(cancellationToken));
            return response;
        }
        finally
        {
            _heartBeatTimer.Start();
        }
    }

    public Task CloseConnectionAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Closing connection.");
        _heartBeatTimer.Stop();

        return _connectionAdapter.IsConnected
            ? _connectionAdapter.DisconnectAsync(cancellationToken)
            : Task.CompletedTask;
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

        await SendRequestAsync<PingRequest, PingResponse>(
            new PingRequest(), (data) => new PingResponse(data), CancellationToken.None);
    }
}