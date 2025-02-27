using ECommerce.Application.UseCases.Queries;
using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Repositories.StoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Handlers
{
    public class GetAllLocationsHandler : IRequestHandler<GetAllLocationsQuery, List<StoreLocationEntity>>
    {
        readonly IStoreLocationRepository _storeLocationRepository;

        public GetAllLocationsHandler(IStoreLocationRepository storeLocationRepository)
        {
            _storeLocationRepository = storeLocationRepository;
        }

        public async Task<List<StoreLocationEntity>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            return await _storeLocationRepository.GetAll();
        }
    }
}