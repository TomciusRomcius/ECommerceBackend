using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Repositories;

public interface ICartProductsRepository
{
    public Task<CartProductEntity?> AddItemAsync(CartProductEntity cartProduct);
    public Task<CartProductEntity?> UpdateItemAsync(CartProductEntity cartProduct);
    public Task RemoveItemAsync(string userId, int productId);
    public Task<List<CartProductEntity>> GetUserCartProductsAsync(string userId);
    public Task<List<CartProductModel>> GetUserCartProductsDetailedAsync(string userId);
    public Task RemoveAllCartItemsAsync(Guid userId);
}