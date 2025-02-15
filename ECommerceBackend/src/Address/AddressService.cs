using ECommerce.DataAccess.Models.ShippingAddress;
using ECommerce.DataAccess.Repositories.ShippingAddress;

namespace ECommerce.Address
{
    public interface IAddressService
    {
        /// <returns>A billing and shipping address</returns>
        public Task<List<ShippingAddressModel>> GetAddresses(string userId);
        public Task AddAddress(ShippingAddressModel address);
        public Task UpdateAddress(UpdateShippingAddressModel updateAddressModel);
        public Task DeleteAddress(string userId, bool isShipping);
    }

    public class AddressService : IAddressService
    {
        readonly IShippingAddressRepository _addressRepository;

        public AddressService(IShippingAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task AddAddress(ShippingAddressModel address)
        {
            await _addressRepository.AddAddressAsync(address);
        }

        public async Task DeleteAddress(string userId, bool isShipping)
        {
            await _addressRepository.DeleteAddressAsync(userId, isShipping);
        }

        public async Task<List<ShippingAddressModel>> GetAddresses(string userId)
        {
            return await _addressRepository.GetAddresses(userId);
        }

        public async Task UpdateAddress(UpdateShippingAddressModel updateAddressModel)
        {
            await _addressRepository.UpdateAddressAsync(updateAddressModel);
        }
    }
}