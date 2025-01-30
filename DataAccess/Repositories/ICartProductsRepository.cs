using ECommerce.DataAccess.Models.CartProduct;

namespace ECommerce.DataAccess.Repositories
{
    public interface ICartProductsRepository
    {
        public Task<CartProductModel?> AddItemAsync(CartProductModel cartProductModel);
        public Task<CartProductModel?> UpdateItemAsync(CartProductModel cartProduct);
        public Task RemoveItemAsync(string userId, int productId);
        public Task<List<CartProductModel>> GetUserCartProductsAsync(string userId);
        public Task RemoveAllCartItemsAsync(string userId);
    }
}