using ECommerce.DataAccess.Models;

namespace ECommerce.DataAccess.Repositories
{
    public interface IManufacturerRepository
    {
        public Task<ManufacturerModel?> CreateAsync(string manufacturerName);
        public Task UpdateAsync(UpdateManufacturerModel updateModel);
        public Task DeleteAsync(int manufacturerId);
        public Task<List<ManufacturerModel>> GetAll();
        public Task<ManufacturerModel?> FindByIdAsync(int manufacturerId);
        public Task<ManufacturerModel?> FindByNameAsync(string name);
    }
}