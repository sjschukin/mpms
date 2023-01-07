using Mpms.Common;

namespace Mpms.Services;

public class ClientService : BackgroundService
{
    private readonly ILogger<ClientService> _logger;
    private readonly IClient _client;

    public ClientService(ILogger<ClientService> logger, IClient client)
    {
        _logger = logger;
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{name} is starting.", nameof(ClientService));

        stoppingToken.Register(OnStopServiceCallback);
        await _client.EstablishConnectionAsync();

        _logger.LogInformation("{name} has stopped.", nameof(ClientService));
    }

    private async void OnStopServiceCallback()
    {
        _logger.LogInformation("{name} is stopping.", nameof(ClientService));
        await _client.CloseConnectionAsync();
    }
}