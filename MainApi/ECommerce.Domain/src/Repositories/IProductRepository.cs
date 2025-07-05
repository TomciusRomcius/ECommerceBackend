using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;

namespace ECommerce.Domain.src.Repositories;

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