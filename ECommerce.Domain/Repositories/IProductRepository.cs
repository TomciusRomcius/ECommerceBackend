using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Repositories;

public interface IProductRepository
{
    public Task<ProductEntity?> CreateAsync(ProductEntity product);
    public Task UpdateAsync(UpdateProductModel product);
    public Task DeleteAsync(int productId);
    public Task<List<ProductEntity>> GetAll();
    public Task<List<ProductEntity>> GetAllInCategory(int categoryId);
    public Task<ProductEntity?> FindByIdAsync(int productId);
    public Task<ProductEntity?> FindByNameAsync(string productName);
}