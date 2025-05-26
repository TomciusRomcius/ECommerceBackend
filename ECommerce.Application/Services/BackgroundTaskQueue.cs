using System.Threading.Channels;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public BackgroundTaskQueue()
    {
        var options = new BoundedChannelOptions(100);
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        await _queue.Writer.WriteAsync(func);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueBackgroundWorkItemAsync(
        CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync();
    }
}