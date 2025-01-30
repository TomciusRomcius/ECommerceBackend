using ECommerce.DataAccess.Models;

namespace ECommerce.DataAccess.Repositories
{
    public interface IProductRepository
    {
        public Task<ProductModel?> CreateAsync(ProductModel product);
        public Task UpdateAsync(UpdateProductModel product);
        public Task DeleteAsync(int productId);
        public Task<List<ProductModel>> GetAll();
        public Task<List<ProductModel>> GetAllInCategory(int categoryId);
        public Task<ProductModel?> FindByIdAsync(int productId);
        public Task<ProductModel?> FindByNameAsync(string productName);
    }
}