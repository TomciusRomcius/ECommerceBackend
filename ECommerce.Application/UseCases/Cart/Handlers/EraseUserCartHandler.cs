using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers;

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