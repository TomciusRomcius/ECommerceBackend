using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Repositories;

public interface ICartProductsRepository
{
    public Task<ResultError?> AddItemAsync(CartProductEntity cartProduct);
    public Task<ResultError?> UpdateItemAsync(CartProductEntity cartProduct);
    public Task<ResultError?> RemoveItemAsync(string userId, int productId);
    public Task<Result<List<CartProductEntity>>> GetUserCartProductsAsync(string userId);
    public Task<Result<List<CartProductModel>>> GetUserCartProductsDetailedAsync(string userId);
    public Task<ResultError?> RemoveAllCartItemsAsync(Guid userId);
}