using Mpms.Common;

namespace Mpms.Services;

public class ClientService : BackgroundService
{
    private const int DELAY_DURATION = 3000;

    private readonly ILogger<ClientService> _logger;
    private readonly IServiceScopeFactory _factory;

    public ClientService(ILogger<ClientService> logger, IServiceScopeFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = _factory.CreateScope();

        try
        {
            _logger.LogInformation("{name} is starting.", nameof(ClientService));

            IClient client = scope.ServiceProvider.GetRequiredService<IClient>();
            stoppingToken.Register(OnStopServiceCallback);
            await client.EstablishConnectionAsync();

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(DELAY_DURATION, CancellationToken.None);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled exception occured.");
            throw;
        }
        finally
        {
            scope.Dispose();
            _logger.LogInformation("{name} has stopped.", nameof(ClientService));
        }
    }

    private void OnStopServiceCallback()
    {
        _logger.LogInformation("{name} is stopping.", nameof(ClientService));
    }
}