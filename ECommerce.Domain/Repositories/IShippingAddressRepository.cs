using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Repositories;

public interface IShippingAddressRepository
{
    public Task AddAddressAsync(ShippingAddressEntity addressEntity);
    public Task UpdateAddressAsync(UpdateShippingAddressModel updateAddressEntity);
    public Task<List<ShippingAddressEntity>> GetAddresses(string userId);
    public Task DeleteAddressAsync(string userId);
}