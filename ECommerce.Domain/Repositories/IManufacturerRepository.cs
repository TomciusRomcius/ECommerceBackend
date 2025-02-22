using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Models.Manufacturer;

namespace ECommerce.Domain.Repositories.Manufacturer
{
    public interface IManufacturerRepository
    {
        public Task<ManufacturerEntity?> CreateAsync(string manufacturerName);
        public Task UpdateAsync(UpdateManufacturerModel updateEntity);
        public Task DeleteAsync(int manufacturerId);
        public Task<List<ManufacturerEntity>> GetAll();
        public Task<ManufacturerEntity?> FindByIdAsync(int manufacturerId);
        public Task<ManufacturerEntity?> FindByNameAsync(string name);
    }
}