using System.Data;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.PaymentSession;

[ApiController]
[Route("[controller]")]
public class PaymentSessionController : ControllerBase
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PaymentSessionController(IBackgroundTaskQueue backgroundTaskQueue, IServiceScopeFactory serviceScopeFactory)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _serviceScopeFactory = serviceScopeFactory;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        string? signature = Request.Headers["Stripe-Signature"];

        Func<CancellationToken, ValueTask> task = async cancellationToken =>
        {
            using IServiceScope? scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var paymentSessionFactory = scope.ServiceProvider.GetService<IPaymentSessionFactory>();

            if (mediator is null || paymentSessionFactory is null)
                throw new DataException("Payment session factory is null");
            // TODO: handle this as the user might have payed but the charge isn't being processesd
            IPaymentSessionService paymentSessionService =
                paymentSessionFactory.CreatePaymentSessionService(PaymentProvider.STRIPE);

            await mediator.Publish(new WebhookNotification(json, signature, paymentSessionService), cancellationToken);
        };

        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(task);

        return Ok();
    }
}