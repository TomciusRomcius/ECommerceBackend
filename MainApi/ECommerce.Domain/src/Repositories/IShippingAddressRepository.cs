using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;

namespace ECommerce.Domain.src.Repositories;

public interface IShippingAddressRepository
{
    public Task AddAddressAsync(ShippingAddressEntity addressEntity);
    public Task UpdateAddressAsync(UpdateShippingAddressModel updateAddressEntity);
    public Task<List<ShippingAddressEntity>> GetAddresses(string userId);
    public Task DeleteAddressAsync(string userId);
}