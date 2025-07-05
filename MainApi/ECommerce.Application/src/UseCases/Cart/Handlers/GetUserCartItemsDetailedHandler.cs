using ECommerce.Application.src.UseCases.Cart.Queries;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

public class GetUserCartItemsDetailedHandler : IRequestHandler<GetUserCartItemsDetailedQuery, Result<List<CartProductModel>>>
{
    private readonly ICartProductsRepository _cartProductsRepository;

    public GetUserCartItemsDetailedHandler(ICartProductsRepository cartProductsRepository)
    {
        _cartProductsRepository = cartProductsRepository;
    }

    public async Task<Result<List<CartProductModel>>> Handle(GetUserCartItemsDetailedQuery request,
        CancellationToken cancellationToken)
    {
        return await _cartProductsRepository.GetUserCartProductsDetailedAsync(request.UserId.ToString());
    }
}