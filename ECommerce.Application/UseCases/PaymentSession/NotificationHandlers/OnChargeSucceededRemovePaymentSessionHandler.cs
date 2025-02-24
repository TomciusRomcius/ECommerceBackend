using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Repositories.PaymentSession;
using MediatR;

public class OnChargeSucceededRemovePaymentSessionHandler : INotificationHandler<ChargeSucceededNotification>
{
    readonly IPaymentSessionRepository _paymentSessionRepository;

    public OnChargeSucceededRemovePaymentSessionHandler(IPaymentSessionRepository paymentSessionRepository)
    {
        _paymentSessionRepository = paymentSessionRepository;
    }

    public async Task Handle(ChargeSucceededNotification notification, CancellationToken cancellationToken)
    {
        await _paymentSessionRepository.DeletePaymentSession(notification.UserId);
    }
}
