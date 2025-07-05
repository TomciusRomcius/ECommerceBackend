using ECommerce.Application.src.UseCases.Cart.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

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