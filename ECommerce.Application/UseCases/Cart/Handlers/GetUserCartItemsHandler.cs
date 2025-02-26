using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Repositories.CartProducts;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers
{
    public class GetUserCartItemsHandler : IRequestHandler<GetUserCartItemsQuery, List<CartProductEntity>>
    {
        readonly ICartProductsRepository _cartProductsRepository;

        public GetUserCartItemsHandler(ICartProductsRepository cartProductsRepository)
        {
            _cartProductsRepository = cartProductsRepository;
        }

        public async Task<List<CartProductEntity>> Handle(GetUserCartItemsQuery request, CancellationToken cancellationToken)
        {
            return await _cartProductsRepository.GetUserCartProductsAsync(request.UserId.ToString());
        }
    }
}