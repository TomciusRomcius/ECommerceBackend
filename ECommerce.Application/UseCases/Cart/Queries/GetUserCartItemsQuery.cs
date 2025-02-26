using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Models.CartProduct;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Queries
{
    public record GetUserCartItemsQuery(Guid UserId) : IRequest<List<CartProductEntity>>;
}