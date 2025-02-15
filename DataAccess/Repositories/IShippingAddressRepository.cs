
using ECommerce.DataAccess.Models.ShippingAddress;

namespace ECommerce.DataAccess.Repositories.ShippingAddress
{
    public interface IShippingAddressRepository
    {
        public Task AddAddressAsync(ShippingAddressModel addressModel);
        public Task UpdateAddressAsync(UpdateShippingAddressModel updateAddressModel);
        public Task<List<ShippingAddressModel>> GetAddresses(string userId);
        public Task DeleteAddressAsync(string userId, bool isShipping);
    }
}