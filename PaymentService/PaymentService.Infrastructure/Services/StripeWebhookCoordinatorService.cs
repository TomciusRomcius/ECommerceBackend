using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Utils;
using PaymentService.Infrastructure.Interfaces;
using Stripe;

namespace PaymentService.Infrastructure.Services;

public class StripeWebhookCoordinatorService : IWebhookCoordinatorService
{
    private readonly WebhookEventStrategyMapContainer _strategyMaps;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ILogger<StripeWebhookCoordinatorService> _logger;

    public StripeWebhookCoordinatorService(WebhookEventStrategyMapContainer strategyMaps,
        IServiceScopeFactory serviceScopeFactory,
        IBackgroundTaskQueue backgroundTaskQueue,
        ILogger<StripeWebhookCoordinatorService> logger)
    {
        _strategyMaps = strategyMaps;
        _serviceScopeFactory = serviceScopeFactory;
        _backgroundTaskQueue = backgroundTaskQueue;
        _logger = logger;
    }

    public async Task HandlePaymentWebhook(string json, string signature)
    {
        Func<CancellationToken, Task> paymentTask = async cancellationToken =>
        {
            using IServiceScope? scope = _serviceScopeFactory.CreateScope();
            var paymentSessionFactory = scope.ServiceProvider.GetRequiredService<IPaymentSessionFactory>();

            IPaymentSessionService paymentSessionService =
                paymentSessionFactory.CreatePaymentSessionService(PaymentProvider.STRIPE);

            Result<Event> result = await paymentSessionService.ParseWebhookEvent<Event>(json, signature);
            if (result.Errors.Any())
            {
                _logger.LogError(result.ErrorsToString());
                return;
            }

            Event paymentEvent = result.GetValue();
            IWebhookEventStrategyMap? strategyMap = _strategyMaps.GetStrategyMap(PaymentProvider.STRIPE);
            if (strategyMap == null)
            {
                _logger.LogError("Failed to retrieve Stripe webhook strategy map");
                return;
            }

            Result<IStripeWebhookStrategy> webhookHandlerStrategyResult = strategyMap.TryGetStrategy<IStripeWebhookStrategy>(paymentEvent.Type);
            if (webhookHandlerStrategyResult.Errors.Any())
            {
                ResultError firstError = webhookHandlerStrategyResult.Errors.First();
                if (firstError.ErrorType == ResultErrorType.UNSUPPORTED
                    && webhookHandlerStrategyResult.Errors.Count() == 1)
                {
                    _logger.LogWarning(firstError.Message);
                }
                else
                {
                    _logger.LogError($"{webhookHandlerStrategyResult.ErrorsToString()}");
                }

                IStripeWebhookStrategy webhookHandlerStrategy = webhookHandlerStrategyResult.GetValue();
                await webhookHandlerStrategy.RunAsync(paymentEvent.Data.Object);
            };
        };

        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(paymentTask);
    }
}