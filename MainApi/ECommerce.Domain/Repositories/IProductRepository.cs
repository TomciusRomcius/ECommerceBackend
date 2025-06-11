using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Repositories;

public interface IProductRepository
{
    public Task<Result<ProductEntity>> CreateAsync(ProductEntity product);
    public Task<ResultError?> UpdateAsync(UpdateProductModel product);
    public Task DeleteAsync(int productId);
    public Task<List<ProductEntity>> GetAll();
    public Task<List<ProductEntity>> GetAllInCategory(int categoryId);
    public Task<ProductEntity?> FindByIdAsync(int productId);
    public Task<ProductEntity?> FindByNameAsync(string productName);
}