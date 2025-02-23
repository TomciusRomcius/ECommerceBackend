using ECommerce.Domain.Entities.PaymentSession;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Commands
{
    public record CreatePaymentSessionCommand(PaymentSessionEntity PaymentSessionEntity) : IRequest;
}