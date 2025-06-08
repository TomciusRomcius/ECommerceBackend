using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers;

public class UpdateCartItemQuantityHandler : IRequestHandler<UpdateCartItemQuantityCommand, ResultError?>
{
    private readonly ICartProductsRepository _cartProductsRepository;

    public UpdateCartItemQuantityHandler(ICartProductsRepository cartProductsRepository)
    {
        _cartProductsRepository = cartProductsRepository;
    }

    public async Task<ResultError?> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        return await _cartProductsRepository.UpdateItemAsync(request.CartProduct);
    }
}