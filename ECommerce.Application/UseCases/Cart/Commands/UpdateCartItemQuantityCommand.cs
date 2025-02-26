using ECommerce.Domain.Entities.CartProduct;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Commands
{
    public record UpdateCartItemQuantityCommand(CartProductEntity CartProduct) : IRequest;
}