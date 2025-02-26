using ECommerce.Domain.Models.CartProduct;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Queries
{
    public record GetUserCartItemsDetailedQuery(Guid UserId) : IRequest<List<CartProductModel>>;
}