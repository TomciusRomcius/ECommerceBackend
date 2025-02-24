using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Models.CartProduct;

namespace ECommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        public Task<List<CartProductEntity>> GetAllUserItems(string userId);
        public Task<List<CartProductModel>> GetAllUserItemsDetailed(string userId);
        public Task<CartProductEntity?> AddItem(CartProductEntity cartProductModel);
        public Task UpdateItemQuantity(CartProductEntity cartProductModel);
        public Task WipeAsync(Guid userId);
    }
}