using ECommerce.Application.Interfaces;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace ECommerce.Application.Services;

public class StripeWebhookCoordinatorService : IWebhookCoordinatorService
{
    private readonly WebhookEventStrategyMapContainer _strategyMaps;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public StripeWebhookCoordinatorService(WebhookEventStrategyMapContainer strategyMaps,
        IServiceScopeFactory serviceScopeFactory,
        IBackgroundTaskQueue backgroundTaskQueue)
    {
        _strategyMaps = strategyMaps;
        _serviceScopeFactory = serviceScopeFactory;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    // TODO: error handling
    public async Task HandlePaymentWebhook(string json, string signature)
    {
        Func<CancellationToken, ValueTask> paymentTask = async cancellationToken =>
        {
            using IServiceScope? scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var paymentSessionFactory = scope.ServiceProvider.GetRequiredService<IPaymentSessionFactory>();

            IPaymentSessionService paymentSessionService =
                paymentSessionFactory.CreatePaymentSessionService(PaymentProvider.STRIPE);

            Result<Event> result = await paymentSessionService.ParseWebhookEvent<Event>(json, signature);
            if (result.Errors.Any())
            {
                // TODO: log
                return;
            }

            Event paymentEvent = result.GetValue();
            IWebhookEventStrategyMap? strategyMap = _strategyMaps.GetStrategyMap(PaymentProvider.STRIPE);
            if (strategyMap == null)
            {
                // TODO: log error
                return;
            }

            Result<IStripeWebhookStrategy> webhookHandlerStrategyResult = strategyMap.TryGetStrategy<IStripeWebhookStrategy>(paymentEvent.Type);
            if (webhookHandlerStrategyResult.Errors.Any())
            {
                // TODO: log error
                return;
            }

            IStripeWebhookStrategy webhookHandlerStrategy = webhookHandlerStrategyResult.GetValue();
            await webhookHandlerStrategy.RunAsync(paymentEvent.Data.Object);
        };

        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(paymentTask);
    }
}