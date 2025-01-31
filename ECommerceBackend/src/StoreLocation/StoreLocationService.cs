using ECommerce.DataAccess.Models.StoreLocation;
using ECommerce.DataAccess.Repositories.StoreLocation;

namespace ECommerce.StoreLocation
{
    public class StoreLocationService : IStoreLocationService
    {
        readonly IStoreLocationRepository _storeLocationRepository;

        public StoreLocationService(IStoreLocationRepository storeLocationRepository)
        {
            _storeLocationRepository = storeLocationRepository;
        }

        public async Task<StoreLocationModel?> CreateStoreLocation(CreateStoreLocationModel storeLocation)
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

        public async Task<List<StoreLocationModel>> GetLocations()
        {
            return await _storeLocationRepository.GetAll();
        }
    }
}
