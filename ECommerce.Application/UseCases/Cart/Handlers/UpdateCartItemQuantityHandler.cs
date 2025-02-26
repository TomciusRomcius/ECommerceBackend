using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Repositories.CartProducts;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers
{
    public class UpdateCartItemQuantityHandler : IRequestHandler<UpdateCartItemQuantityCommand>
    {
        readonly ICartProductsRepository _cartProductsRepository;

        public UpdateCartItemQuantityHandler(ICartProductsRepository cartProductsRepository)
        {
            _cartProductsRepository = cartProductsRepository;
        }

        public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            await _cartProductsRepository.UpdateItemAsync(request.CartProduct);
        }
    }
}