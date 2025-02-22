using ECommerce.Domain.Entities.Category;
using ECommerce.Domain.Models.Category;

namespace ECommerce.Domain.Repositories.Category
{
    public interface ICategoryRepository
    {
        public Task<CategoryEntity?> CreateAsync(string categoryName);
        public Task UpdateAsync(UpdateCategoryModel updateEntity);
        public Task DeleteAsync(int categoryId);
        public Task<List<CategoryEntity>> GetAll();
        public Task<CategoryEntity?> FindByIdAsync(int categoryId);
        public Task<CategoryEntity?> FindByNameAsync(string categoryName);
    }
}