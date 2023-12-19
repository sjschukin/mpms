using Mpms.Common;

namespace Mpms.Services;

public class ClientBackgroundService(ILogger<ClientBackgroundService> logger, IClient client)
    : BackgroundService
{
    private const int DELAY_DURATION = 3000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("{name} is starting.", nameof(ClientBackgroundService));

            stoppingToken.Register(OnStopServiceCallback);
            await client.EstablishConnectionAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(DELAY_DURATION, stoppingToken);

            await client.CloseConnectionAsync(CancellationToken.None);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception occured.");
            throw;
        }
        finally
        {
            client.Dispose();
            logger.LogInformation("{name} has stopped.", nameof(ClientBackgroundService));
        }
    }

    private void OnStopServiceCallback()
    {
        logger.LogInformation("{name} is stopping.", nameof(ClientBackgroundService));
    }
}