using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client;

public class TcpIpAdapter : IConnectionAdapter
{
    private readonly ILogger<TcpIpAdapter> _logger;
    private readonly MpdConnectionOptions _configuration;
    private readonly TcpClient _client = new();

    public TcpIpAdapter(ILogger<TcpIpAdapter> logger, IOptions<MpdConnectionOptions> configuration)
    {
        _logger = logger;
        _configuration = configuration.Value;
    }

    public bool IsConnected => _client.Connected;

    public async Task<Stream> CreateStreamAsync(CancellationToken cancellationToken)
    {
        IPHostEntry entry = await Dns.GetHostEntryAsync(_configuration.Address, cancellationToken);
        IPAddress? address = entry.AddressList.FirstOrDefault();

        if (address is null)
            throw new Exception("Cannot resolve host address.");

        var endPoint = new IPEndPoint(address, _configuration.Port);
        _client.SendTimeout = _configuration.CommandTimeout * 1000;
        _client.ReceiveTimeout = _configuration.CommandTimeout * 1000;

        await _client.ConnectAsync(endPoint, cancellationToken);

        return _client.GetStream();
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken)
    {
        await _client.Client.DisconnectAsync(true, cancellationToken);
    }

    public void Dispose()
    {
        _client.Close();
        _client.Dispose();
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        _client.Close();
        _client.Dispose();
        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }
}