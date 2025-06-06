using System.Data;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Services;

public class WebhookCoordinatorService : IWebhookCoordinatorService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    
    public WebhookCoordinatorService(IServiceScopeFactory serviceScopeFactory, IBackgroundTaskQueue backgroundTaskQueue)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _backgroundTaskQueue = backgroundTaskQueue;
    }
    
    public async Task HandlePaymentWebhook(string json, string signature)
    {
        Func<CancellationToken, ValueTask> paymentTask = async cancellationToken =>
        {
            using IServiceScope? scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var paymentSessionFactory = scope.ServiceProvider.GetService<IPaymentSessionFactory>();
            
            if (mediator == null)
            {
                throw new DataException("Payment session is null");
            }

            if (paymentSessionFactory == null)
            {
                throw new DataException("Payment session factory is null");
            }

            IPaymentSessionService paymentSessionService =
                paymentSessionFactory.CreatePaymentSessionService(PaymentProvider.STRIPE);

            await mediator.Publish(new WebhookNotification(json, signature, paymentSessionService), cancellationToken);
        };

        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(paymentTask);
    }
}