using ECommerce.DataAccess.Entities.CartProduct;
using ECommerce.DataAccess.Models.CartProduct;
using ECommerce.DataAccess.Repositories;

namespace ECommerce.Cart
{
    public interface ICartService
    {
        public Task<List<CartProductModel>> GetAllUserItems(string userId);
        public Task<CartProductEntity?> AddItem(CartProductEntity cartProductModel);
        public Task UpdateItemQuantity(CartProductEntity cartProductModel);
    }

    public class CartService : ICartService
    {
        readonly ICartProductsRepository _cartProductsRepository;

        public CartService(ICartProductsRepository cartProductsRepository)
        {
            _cartProductsRepository = cartProductsRepository;
        }

        public async Task<CartProductEntity?> AddItem(CartProductEntity cartProductModel)
        {
            return await _cartProductsRepository.AddItemAsync(cartProductModel);
        }

        public async Task<List<CartProductModel>> GetAllUserItems(string userId)
        {
            return await _cartProductsRepository.GetUserCartProductsAsync(userId);
        }

        public async Task UpdateItemQuantity(CartProductEntity cartProductModel)
        {
            await _cartProductsRepository.UpdateItemAsync(cartProductModel);
        }
    }
}