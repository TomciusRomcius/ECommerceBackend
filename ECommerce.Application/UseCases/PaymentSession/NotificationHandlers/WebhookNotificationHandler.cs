using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Models.PaymentSession;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.NotificationHandlers
{
    public class WebhookNotificationHandler : INotificationHandler<WebhookNotification>
    {
        readonly IMediator _mediator;

        public WebhookNotificationHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(WebhookNotification notification, CancellationToken cancellationToken)
        {

            PaymentProviderEvent? ev = await notification._paymentSessionService.ParseWebhookEvent(notification.Json, notification.Signature);
            if (ev is null)
            {
                return;
            }

            if (ev.EventType == PaymentProviderEventType.CHARGE_SUCEEDED)
            {
                await _mediator.Publish(new ChargeSucceededNotification(new Guid(ev.UserId)));
                // publish event
            }
        }
    }
}