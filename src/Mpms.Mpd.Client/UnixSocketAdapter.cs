using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client;

public class UnixSocketAdapter : IConnectionAdapter
{
    private readonly ILogger<UnixSocketAdapter> _logger;
    private readonly MpdConnectionOptions _configuration;
    private readonly Socket _socket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

    public UnixSocketAdapter(ILogger<UnixSocketAdapter> logger,
        IOptions<MpdConnectionOptions> configuration)
    {
        _logger = logger;
        _configuration = configuration.Value;
    }

    public bool IsConnected => _socket.Connected;

    public async Task<Stream> CreateStreamAsync(CancellationToken cancellationToken)
    {
        var endPoint = new UnixDomainSocketEndPoint(_configuration.Address);
        _socket.SendTimeout = _configuration.CommandTimeout * 1000;
        _socket.ReceiveTimeout = _configuration.CommandTimeout * 1000;

        await _socket.ConnectAsync(endPoint, cancellationToken);

        return new NetworkStream(_socket);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken)
    {
        await _socket.DisconnectAsync(true, cancellationToken);
    }

    public void Dispose()
    {
        if (_socket.Connected)
            _socket.Disconnect(false);

        _socket.Close();
        _socket.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_socket.Connected)
            await _socket.DisconnectAsync(false);

        _socket.Close();
        _socket.Dispose();
        GC.SuppressFinalize(this);
    }
}