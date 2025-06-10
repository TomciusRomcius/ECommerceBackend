using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Repositories;

public interface IManufacturerRepository
{
    public Task<Result<int>> CreateAsync(string manufacturerName);
    public Task<ResultError?> UpdateAsync(UpdateManufacturerModel updateEntity);
    public Task DeleteAsync(int manufacturerId);
    public Task<List<ManufacturerEntity>> GetAll();
    public Task<ManufacturerEntity?> FindByIdAsync(int manufacturerId);
    public Task<ManufacturerEntity?> FindByNameAsync(string name);
}