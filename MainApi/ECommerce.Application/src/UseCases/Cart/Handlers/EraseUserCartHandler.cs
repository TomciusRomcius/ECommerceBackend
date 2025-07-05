using ECommerce.Application.src.UseCases.Cart.Commands;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

public class EraseUserCartHandler : IRequestHandler<EraseUserCartCommand>
{
    private readonly ICartProductsRepository _cartProductsRepository;

    public EraseUserCartHandler(ICartProductsRepository cartProductsRepository)
    {
        _cartProductsRepository = cartProductsRepository;
    }

    public async Task Handle(EraseUserCartCommand request, CancellationToken cancellationToken)
    {
        await _cartProductsRepository.RemoveAllCartItemsAsync(request.UserId);
    }
}