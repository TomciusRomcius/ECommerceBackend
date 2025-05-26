using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Repositories;

public interface IManufacturerRepository
{
    public Task<ManufacturerEntity?> CreateAsync(string manufacturerName);
    public Task UpdateAsync(UpdateManufacturerModel updateEntity);
    public Task DeleteAsync(int manufacturerId);
    public Task<List<ManufacturerEntity>> GetAll();
    public Task<ManufacturerEntity?> FindByIdAsync(int manufacturerId);
    public Task<ManufacturerEntity?> FindByNameAsync(string name);
}