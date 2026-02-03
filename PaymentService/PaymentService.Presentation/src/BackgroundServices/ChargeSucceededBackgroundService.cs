using PaymentService.Infrastructure.Interfaces;

namespace PaymentService.Presentation.BackgroundServices;

public class ChargeSucceededBackgroundService : BackgroundService
{
    private readonly IChargeSucceededConsumer _chargeSucceededConsumer;

    public ChargeSucceededBackgroundService(IChargeSucceededConsumer chargeSucceededConsumer)
    {
        _chargeSucceededConsumer = chargeSucceededConsumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () => await _chargeSucceededConsumer.StartAsync(stoppingToken), stoppingToken);
    }
}
