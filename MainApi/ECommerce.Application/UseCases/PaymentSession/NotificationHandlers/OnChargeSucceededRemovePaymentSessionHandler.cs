using ECommerce.Application.UseCases.Common.Notifications;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.NotificationHandlers;

public class OnChargeSucceededRemovePaymentSessionHandler : INotificationHandler<ChargeSucceededNotification>
{
    private readonly IPaymentSessionRepository _paymentSessionRepository;

    public OnChargeSucceededRemovePaymentSessionHandler(IPaymentSessionRepository paymentSessionRepository)
    {
        _paymentSessionRepository = paymentSessionRepository;
    }

    public async Task Handle(ChargeSucceededNotification notification, CancellationToken cancellationToken)
    {
        await _paymentSessionRepository.DeletePaymentSession(notification.UserId);
    }
}