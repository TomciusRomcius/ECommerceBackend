using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Handlers;

public class GetUserCartItemsHandler : IRequestHandler<GetUserCartItemsQuery, Result<List<CartProductEntity>>>
{
    private readonly ICartProductsRepository _cartProductsRepository;

    public GetUserCartItemsHandler(ICartProductsRepository cartProductsRepository)
    {
        _cartProductsRepository = cartProductsRepository;
    }

    public async Task<Result<List<CartProductEntity>>> Handle(GetUserCartItemsQuery request,
        CancellationToken cancellationToken)
    {
        return await _cartProductsRepository.GetUserCartProductsAsync(request.UserId.ToString());
    }
}