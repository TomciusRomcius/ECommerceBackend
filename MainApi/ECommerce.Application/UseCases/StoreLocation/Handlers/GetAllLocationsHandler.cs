using ECommerce.Application.UseCases.StoreLocation.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Handlers;

public class GetAllLocationsHandler : IRequestHandler<GetAllLocationsQuery, List<StoreLocationEntity>>
{
    private readonly IStoreLocationRepository _storeLocationRepository;

    public GetAllLocationsHandler(IStoreLocationRepository storeLocationRepository)
    {
        _storeLocationRepository = storeLocationRepository;
    }

    public async Task<List<StoreLocationEntity>> Handle(GetAllLocationsQuery request,
        CancellationToken cancellationToken)
    {
        return await _storeLocationRepository.GetAll();
    }
}