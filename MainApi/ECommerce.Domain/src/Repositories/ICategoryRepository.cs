using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;

namespace ECommerce.Domain.src.Repositories;

public interface ICategoryRepository
{
    public Task<Result<int>> CreateAsync(string categoryName);
    public Task<ResultError?> UpdateAsync(UpdateCategoryModel updateEntity);
    public Task DeleteAsync(int categoryId);
    public Task<List<CategoryEntity>> GetAll();
    public Task<CategoryEntity?> FindByIdAsync(int categoryId);
    public Task<CategoryEntity?> FindByNameAsync(string categoryName);
}