using ECommerce.Application.UseCases.Common.Notifications;
using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.NotificationHandlers;

public class WebhookNotificationHandler : INotificationHandler<WebhookNotification>
{
    private readonly IMediator _mediator;

    public WebhookNotificationHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(WebhookNotification notification, CancellationToken cancellationToken)
    {
        Result<PaymentProviderEvent> result = await notification._paymentSessionService.ParseWebhookEvent(notification.Json, notification.Signature);
        if (result.Errors.Any())
        {
            // TODO: handle
            return;
        }

        PaymentProviderEvent ev = result.GetValue();

        if (ev.EventType == PaymentProviderEventType.CHARGE_SUCEEDED)
            await _mediator.Publish(new ChargeSucceededNotification(new Guid(ev.UserId)), cancellationToken);
    }
}