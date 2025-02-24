using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Models.CartProduct;
using ECommerce.Domain.Repositories.CartProducts;

namespace ECommerce.Application.Services
{
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

        public async Task<List<CartProductEntity>> GetAllUserItems(string userId)
        {
            return await _cartProductsRepository.GetUserCartProductsAsync(userId);
        }

        public async Task<List<CartProductModel>> GetAllUserItemsDetailed(string userId)
        {
            return await _cartProductsRepository.GetUserCartProductsDetailedAsync(userId);
        }

        public async Task UpdateItemQuantity(CartProductEntity cartProductModel)
        {
            await _cartProductsRepository.UpdateItemAsync(cartProductModel);
        }

        public async Task WipeAsync(Guid userId)
        {
            await _cartProductsRepository.RemoveAllCartItemsAsync(userId);
        }
    }
}