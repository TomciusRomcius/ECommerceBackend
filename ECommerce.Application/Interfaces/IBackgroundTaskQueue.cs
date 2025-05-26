namespace ECommerce.Application.Interfaces;

public interface IBackgroundTaskQueue
{
    public ValueTask QueueBackgroundWorkItemAsync(
        Func<CancellationToken, ValueTask> func
    );

    public ValueTask<Func<CancellationToken, ValueTask>> DequeueBackgroundWorkItemAsync(
        CancellationToken cancellationToken
    );
}