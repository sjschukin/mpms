using Mpms.Common;

namespace Mpms.Services;

public class ClientBackgroundService : BackgroundService
{
    private readonly ILogger<ClientBackgroundService> _logger;
    private readonly IClient _client;

    public ClientBackgroundService(ILogger<ClientBackgroundService> logger, IClient client)
    {
        _logger = logger;
        _client = client;
    }

    private const int DELAY_DURATION = 3000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("{name} is starting.", nameof(ClientBackgroundService));

            stoppingToken.Register(OnStopServiceCallback);

            _logger.LogInformation("Establishing connection...");

            await _client.EstablishConnectionAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(DELAY_DURATION, stoppingToken);

            _logger.LogInformation("Closing connection...");

            await _client.CloseConnectionAsync(CancellationToken.None);

            _logger.LogInformation("Connection closed.");
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("Execution was cancelled by the user.");
        }
        catch (Exception e) when(e is not TaskCanceledException)
        {
            _logger.LogError(e, "Unhandled exception occurred.");
            throw;
        }
        finally
        {
            _client.Dispose();
            _logger.LogInformation("{name} has stopped.", nameof(ClientBackgroundService));
        }
    }

    private void OnStopServiceCallback()
    {
        _logger.LogInformation("{name} is stopping...", nameof(ClientBackgroundService));
    }
}