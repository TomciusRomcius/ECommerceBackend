using StoreService.Application.Interfaces;
using StoreService.Application.Services;

namespace StoreService.Presentation.Jobs;

public class BackgroundChargeSucceededConsumer : IHostedService
{
    private readonly ILogger<BackgroundChargeSucceededConsumer> _logger;
    private readonly IChargeSucceededConsumer _chargeSucceededConsumer;

    public BackgroundChargeSucceededConsumer(ILogger<BackgroundChargeSucceededConsumer> logger, IChargeSucceededConsumer chargeSucceededConsumer)
    {
        _logger = logger;
        _chargeSucceededConsumer = chargeSucceededConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _chargeSucceededConsumer.TryConsumeAndHandle(cancellationToken);
            }
        },
        cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
