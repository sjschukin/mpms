using Mpms.Common;

namespace Mpms.Services;

public class ClientService(ILogger<ClientService> logger, IServiceScopeFactory factory)
    : BackgroundService
{
    private const int DELAY_DURATION = 3000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = factory.CreateScope();

        try
        {
            logger.LogInformation("{name} is starting.", nameof(ClientService));

            IClient client = scope.ServiceProvider.GetRequiredService<IClient>();
            stoppingToken.Register(OnStopServiceCallback);
            await client.EstablishConnectionAsync();

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(DELAY_DURATION, CancellationToken.None);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception occured.");
            throw;
        }
        finally
        {
            scope.Dispose();
            logger.LogInformation("{name} has stopped.", nameof(ClientService));
        }
    }

    private void OnStopServiceCallback()
    {
        logger.LogInformation("{name} is stopping.", nameof(ClientService));
    }
}