using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;

namespace ECommerce.Domain.src.Repositories;

public interface IManufacturerRepository
{
    public Task<Result<int>> CreateAsync(string manufacturerName);
    public Task<ResultError?> UpdateAsync(UpdateManufacturerModel updateEntity);
    public Task DeleteAsync(int manufacturerId);
    public Task<List<ManufacturerEntity>> GetAll();
    public Task<ManufacturerEntity?> FindByIdAsync(int manufacturerId);
    public Task<ManufacturerEntity?> FindByNameAsync(string name);
}