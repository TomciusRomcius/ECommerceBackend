using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Repositories;

public interface ICategoryRepository
{
    public Task<Result<int>> CreateAsync(string categoryName);
    public Task<ResultError?> UpdateAsync(UpdateCategoryModel updateEntity);
    public Task DeleteAsync(int categoryId);
    public Task<List<CategoryEntity>> GetAll();
    public Task<CategoryEntity?> FindByIdAsync(int categoryId);
    public Task<CategoryEntity?> FindByNameAsync(string categoryName);
}