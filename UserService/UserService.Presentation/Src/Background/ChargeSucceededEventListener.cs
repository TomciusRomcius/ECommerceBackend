using UserService.Application.Services;

namespace UserService.Presentation.Background;

public class ChargeSucceededBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ChargeSucceededBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
            {
                await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();
                await scope.ServiceProvider
                    .GetRequiredService<IChargeSucceededEventListener>()
                    .StartAsync(stoppingToken);
            },
            stoppingToken
        );
    }
}
