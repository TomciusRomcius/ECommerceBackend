using ECommerce.Application.src.UseCases.Cart.Commands;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, ResultError?>
{
    private readonly ICartProductsRepository _cartProductsRepository;

    public AddItemToCartHandler(ICartProductsRepository cartProductsRepository)
    {
        _cartProductsRepository = cartProductsRepository;
    }

    public async Task<ResultError?> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        ResultError? error = await _cartProductsRepository.AddItemAsync(request.cartProduct);
        return error;
    }
}