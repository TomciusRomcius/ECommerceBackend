using ECommerce.Application.Interfaces;

namespace ECommerce.Presentation.Common.Services;

public class IoBackgroundTaskRunner : BackgroundService
{
    private readonly IBackgroundTaskQueue _backgroundQueue;
    private readonly ILogger _logger;
    public static int TaskBatchSize = 5;

    public IoBackgroundTaskRunner(IBackgroundTaskQueue backgroundQueue, ILogger logger)
    {
        _backgroundQueue = backgroundQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                List<Task> tasks = [];

                for (int i = 0; i < TaskBatchSize; i++)
                {
                    Func<CancellationToken, Task> func = await _backgroundQueue.DequeueBackgroundWorkItemAsync(stoppingToken);
                    tasks.Add(func(stoppingToken));
                }

                await Task.WhenAll(tasks);
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
}