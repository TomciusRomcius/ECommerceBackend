using ECommerce.DataAccess.Entities.CartProduct;
using ECommerce.DataAccess.Models.CartProduct;

namespace ECommerce.DataAccess.Repositories
{
    public interface ICartProductsRepository
    {
        public Task<CartProductEntity?> AddItemAsync(CartProductEntity cartProduct);
        public Task<CartProductEntity?> UpdateItemAsync(CartProductEntity cartProduct);
        public Task RemoveItemAsync(string userId, int productId);

        public Task<List<CartProductEntity>> GetUserCartProductsAsync(string userId);
        public Task<List<CartProductModel>> GetUserCartProductsDetailedAsync(string userId);
        public Task RemoveAllCartItemsAsync(Guid userId);
    }
}