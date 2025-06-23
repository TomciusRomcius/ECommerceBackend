using ECommerce.Application.EventTypes;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.NotificationHandlers;

public class OnChargeSucceededRemovePaymentSessionHandler : INotificationHandler<ChargeSucceededEvent>
{
    private readonly IPaymentSessionRepository _paymentSessionRepository;

    public OnChargeSucceededRemovePaymentSessionHandler(IPaymentSessionRepository paymentSessionRepository)
    {
        _paymentSessionRepository = paymentSessionRepository;
    }

    public async Task Handle(ChargeSucceededEvent notification, CancellationToken cancellationToken)
    {
        await _paymentSessionRepository.DeletePaymentSession(new Guid(notification.UserId));
    }
}