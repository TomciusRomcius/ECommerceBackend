using ECommerce.Domain.Enums.PaymentProvider;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Commands
{
    public record CreatePaymentSessionCommand(
        Guid UserId,
        string PaymentSessionId,
        PaymentProvider Provider
    ) : IRequest;
}