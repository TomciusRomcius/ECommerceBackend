using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Entities.PaymentSession;
using MediatR;

namespace ECommerce.Application.UseCases.Order.Commands
{
    public record CreateOrderPaymentSessionCommand(Guid UserId, List<CartProductEntity> CartItems, Dictionary<int, int> ProductStock) : IRequest<PaymentSessionEntity>;
}