using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client;

public class UnixSocketAdapter(
    ILogger<UnixSocketAdapter> logger,
    IOptions<MpdConnectionOptions> configuration) : IConnectionAdapter
{
    private readonly ILogger<UnixSocketAdapter> _logger = logger;
    private readonly MpdConnectionOptions _configuration = configuration.Value;
    private readonly Socket _socket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

    public bool IsConnected => _socket.Connected;

    public async Task<Stream> CreateStreamAsync()
    {
        var endPoint = new UnixDomainSocketEndPoint(_configuration.Address);
        _socket.SendTimeout = _configuration.CommandTimeout * 1000;
        _socket.ReceiveTimeout = _configuration.CommandTimeout * 1000;

        await _socket.ConnectAsync(endPoint)
            .ConfigureAwait(false);

        return new NetworkStream(_socket);
    }

    public async Task DisconnectAsync()
    {
        await _socket.DisconnectAsync(true);
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
            await _socket.DisconnectAsync(false)
                .ConfigureAwait(false);

        _socket.Close();
        _socket.Dispose();
        GC.SuppressFinalize(this);
    }
}