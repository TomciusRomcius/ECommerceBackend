using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers;

public class UpdateCartItemQuantityHandler : IRequestHandler<UpdateCartItemQuantityCommand>
{
    private readonly ICartProductsRepository _cartProductsRepository;

    public UpdateCartItemQuantityHandler(ICartProductsRepository cartProductsRepository)
    {
        _cartProductsRepository = cartProductsRepository;
    }

    public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        await _cartProductsRepository.UpdateItemAsync(request.CartProduct);
    }
}