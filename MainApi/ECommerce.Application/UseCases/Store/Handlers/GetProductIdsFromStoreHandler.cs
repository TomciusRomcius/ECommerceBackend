using ECommerce.Application.UseCases.Store.Queries;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Handlers;

public class GetProductIdsFromStoreHandler : IRequestHandler<GetProductIdsFromStoreQuery, List<int>>
{
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public GetProductIdsFromStoreHandler(IProductStoreLocationRepository productStoreLocationRepository)
    {
        _productStoreLocationRepository = productStoreLocationRepository;
    }

    public async Task<List<int>> Handle(GetProductIdsFromStoreQuery request, CancellationToken cancellationToken)
    {
        return await _productStoreLocationRepository.GetProductIdsFromStoreAsync(request.StoreLocationId);
    }
}