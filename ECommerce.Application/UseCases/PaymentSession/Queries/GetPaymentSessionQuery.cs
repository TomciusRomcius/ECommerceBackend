using ECommerce.Domain.Entities.PaymentSession;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Queries
{
    public record GetPaymentSessionQuery(Guid UserId) : IRequest<PaymentSessionEntity?>;
}