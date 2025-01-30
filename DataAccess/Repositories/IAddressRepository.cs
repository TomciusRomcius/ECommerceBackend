using ECommerce.DataAccess.Models.Address;

namespace ECommerce.DataAccess.Repositories
{
    public interface IAddressRepository
    {
        public Task AddAddressAsync(AddressModel addressModel);
        public Task UpdateAddressAsync(UpdateAddressModel updateAddressModel);
        public Task<List<AddressModel>> GetAddresses(string userId);
        public Task DeleteAddressAsync(string userId, bool isShipping);
    }
}