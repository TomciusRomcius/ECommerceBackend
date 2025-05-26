using ECommerce.Application.UseCases.Store.Queries;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Handlers;

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