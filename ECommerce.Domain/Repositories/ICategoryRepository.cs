using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Repositories;

public interface ICategoryRepository
{
    public Task<CategoryEntity?> CreateAsync(string categoryName);
    public Task UpdateAsync(UpdateCategoryModel updateEntity);
    public Task DeleteAsync(int categoryId);
    public Task<List<CategoryEntity>> GetAll();
    public Task<CategoryEntity?> FindByIdAsync(int categoryId);
    public Task<CategoryEntity?> FindByNameAsync(string categoryName);
}