using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client;

public class TcpIpAdapter(ILogger<TcpIpAdapter> logger, IOptions<MpdConnectionOptions> configuration)
    : IConnectionAdapter
{
    private readonly ILogger<TcpIpAdapter> _logger = logger;
    private readonly MpdConnectionOptions _configuration = configuration.Value;
    private readonly TcpClient _client = new();

    public bool IsConnected => _client.Connected;

    public async Task<Stream> CreateStreamAsync()
    {
        IPHostEntry entry = await Dns.GetHostEntryAsync(_configuration.Address)
            .ConfigureAwait(false);
        IPAddress? address = entry.AddressList.FirstOrDefault();

        if (address is null)
            throw new Exception("Cannot resolve host address.");

        var endPoint = new IPEndPoint(address, _configuration.Port);
        _client.SendTimeout = _configuration.CommandTimeout * 1000;
        _client.ReceiveTimeout = _configuration.CommandTimeout * 1000;

        await _client.ConnectAsync(endPoint)
            .ConfigureAwait(false);

        return _client.GetStream();
    }

    public async Task DisconnectAsync()
    {
        await _client.Client.DisconnectAsync(true);
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