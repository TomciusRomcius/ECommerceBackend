using ECommerce.Application.src.UseCases.Store.Queries;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Handlers;

public class GetProductsFromStoreHandler : IRequestHandler<GetProductsFromStoreQuery, List<DetailedProductModel>>
{
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public GetProductsFromStoreHandler(IProductStoreLocationRepository productStoreLocationRepository)
    {
        _productStoreLocationRepository = productStoreLocationRepository;
    }

    public async Task<List<DetailedProductModel>> Handle(GetProductsFromStoreQuery request,
        CancellationToken cancellationToken)
    {
        return await _productStoreLocationRepository.GetProductsFromStoreAsync(request.StoreLocationId);
    }
}