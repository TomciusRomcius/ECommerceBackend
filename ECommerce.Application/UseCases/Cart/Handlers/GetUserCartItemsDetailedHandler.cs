using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers;

public class GetUserCartItemsDetailedHandler : IRequestHandler<GetUserCartItemsDetailedQuery, List<CartProductModel>>
{
    private readonly ICartProductsRepository _cartProductsRepository;

    public GetUserCartItemsDetailedHandler(ICartProductsRepository cartProductsRepository)
    {
        _cartProductsRepository = cartProductsRepository;
    }

    public async Task<List<CartProductModel>> Handle(GetUserCartItemsDetailedQuery request,
        CancellationToken cancellationToken)
    {
        return await _cartProductsRepository.GetUserCartProductsDetailedAsync(request.UserId.ToString());
    }
}