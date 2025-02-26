using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Repositories.CartProducts;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers
{
    public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, CartProductEntity?>
    {
        readonly ICartProductsRepository _cartProductsRepository;

        public AddItemToCartHandler(ICartProductsRepository cartProductsRepository)
        {
            _cartProductsRepository = cartProductsRepository;
        }

        public async Task<CartProductEntity?> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            return await _cartProductsRepository.AddItemAsync(request.cartProduct);
        }
    }
}