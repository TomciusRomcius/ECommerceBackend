using ECommerce.DataAccess.Models.Category;

namespace ECommerce.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        public Task<CategoryModel?> CreateAsync(string categoryName);
        public Task UpdateAsync(UpdateCategoryModel updateModel);
        public Task DeleteAsync(int categoryId);
        public Task<List<CategoryModel>> GetAll();
        public Task<CategoryModel?> FindByIdAsync(int categoryId);
        public Task<CategoryModel?> FindByNameAsync(string categoryName);
    }
}