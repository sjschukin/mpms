using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mpms.Common;

namespace Mpms.MpdClient;

public class TcpIpAdapter : IConnectionAdapter
{
    private readonly ILogger<TcpIpAdapter> _logger;
    private readonly MpdConnectionOptions _configuration;
    private readonly TcpClient _client;

    public TcpIpAdapter(ILogger<TcpIpAdapter> logger, IOptions<MpdConnectionOptions> configuration)
    {
        _logger = logger;
        _configuration = configuration.Value;
        _client = new TcpClient();
    }

    public async Task<Stream> CreateStreamAsync()
    {
        IPHostEntry entry = await Dns.GetHostEntryAsync(_configuration.Address)
            .ConfigureAwait(false);
        IPAddress? address = entry.AddressList.FirstOrDefault();

        if (address is null)
            throw new Exception("Cannot resolve host address.");

        var endPoint = new IPEndPoint(address, _configuration.Port);
        _client.SendTimeout = _configuration.CommandTimeout;
        _client.ReceiveTimeout = _configuration.CommandTimeout;

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