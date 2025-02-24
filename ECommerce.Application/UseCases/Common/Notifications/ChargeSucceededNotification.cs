using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Notifications
{
    public record ChargeSucceededNotification(Guid UserId) : INotification;
}