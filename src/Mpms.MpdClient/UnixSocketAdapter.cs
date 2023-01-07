using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Common;

namespace Mpms.MpdClient;

public class UnixSocketAdapter : IConnectionAdapter
{
    private readonly ILogger<UnixSocketAdapter> _logger;
    private readonly MpdConnectionOptions _configuration;
    private readonly Socket _socket;

    public UnixSocketAdapter(ILogger<UnixSocketAdapter> logger, IOptions<MpdConnectionOptions> configuration)
    {
        _logger = logger;
        _configuration = configuration.Value;
        _socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
    }

    public bool Connected => _socket.Connected;

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