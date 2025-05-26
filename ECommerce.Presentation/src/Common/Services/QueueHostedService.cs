using ECommerce.Application.Interfaces;

namespace ECommerce.Presentation.Common.Services;

public class QueueHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _backgroundQueue;
    private readonly ILogger _logger;

    public QueueHostedService(IBackgroundTaskQueue backgroundQueue, ILogger logger)
    {
        _backgroundQueue = backgroundQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                Func<CancellationToken, ValueTask> func =
                    await _backgroundQueue.DequeueBackgroundWorkItemAsync(stoppingToken);
                await func(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogTrace("Operation cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
    }
}