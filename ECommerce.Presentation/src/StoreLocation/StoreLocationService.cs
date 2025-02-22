using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Models.StoreLocation;
using ECommerce.Domain.Repositories.StoreLocation;

namespace ECommerce.StoreLocation
{
    public class StoreLocationService : IStoreLocationService
    {
        readonly IStoreLocationRepository _storeLocationRepository;

        public StoreLocationService(IStoreLocationRepository storeLocationRepository)
        {
            _storeLocationRepository = storeLocationRepository;
        }

        public async Task<StoreLocationEntity?> CreateStoreLocation(CreateStoreLocationModel storeLocation)
        {
            return await _storeLocationRepository.CreateAsync(storeLocation);
        }


        public async Task UpdateStoreLocation(UpdateStoreLocationModel updateStoreLocation)
        {
            await _storeLocationRepository.UpdateAsync(updateStoreLocation);
        }

        public async Task RemoveLocation(int storeLocationId)
        {
            await _storeLocationRepository.DeleteAsync(storeLocationId);
        }

        public async Task<List<StoreLocationEntity>> GetLocations()
        {
            return await _storeLocationRepository.GetAll();
        }
    }
}
