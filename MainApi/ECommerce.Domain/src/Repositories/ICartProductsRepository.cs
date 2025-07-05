using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;

namespace ECommerce.Domain.src.Repositories;

public interface ICartProductsRepository
{
    public Task<ResultError?> AddItemAsync(CartProductEntity cartProduct);
    public Task<ResultError?> UpdateItemAsync(CartProductEntity cartProduct);
    public Task<ResultError?> RemoveItemAsync(string userId, int productId);
    public Task<Result<List<CartProductEntity>>> GetUserCartProductsAsync(string userId);
    public Task<Result<List<CartProductModel>>> GetUserCartProductsDetailedAsync(string userId);
    public Task<ResultError?> RemoveAllCartItemsAsync(Guid userId);
}