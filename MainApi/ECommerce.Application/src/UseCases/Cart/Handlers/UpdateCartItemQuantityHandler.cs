using ECommerce.Application.src.UseCases.Cart.Commands;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

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