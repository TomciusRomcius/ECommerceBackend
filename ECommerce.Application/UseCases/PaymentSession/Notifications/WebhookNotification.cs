using ECommerce.Domain.Interfaces.Services;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Notifications
{
    public record WebhookNotification(string Json, string Signature, IPaymentSessionService _paymentSessionService) : INotification;
}