using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Repositories;

public class CartProductsRepository : ICartProductsRepository
{
    private readonly IPostgresService _postgresService;

    public CartProductsRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task<CartProductEntity?> AddItemAsync(CartProductEntity cartProduct)
    {
        var query = @"
                INSERT INTO cartProducts (userId, productId, storeLocationId, quantity)
                VALUES ($1, $2, $3, $4);
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(cartProduct.UserId)),
            new(cartProduct.ProductId),
            new(cartProduct.StoreLocationId),
            new(cartProduct.Quantity)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);

        return cartProduct;
    }

    public async Task<List<CartProductModel>> GetUserCartProductsDetailedAsync(string userId)
    {
        var query = @"
                SELECT cartProducts.userid, cartProducts.productid, cartProducts.storelocationid, cartProducts.quantity, products.price 
                FROM cartProducts 
                INNER JOIN products
                ON products.productId = cartProducts.productId
                WHERE userId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(userId))
        ];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        var result = new List<CartProductModel>();

        foreach (Dictionary<string, object> row in rows)
            result.Add(new CartProductModel(
                row.GetColumn<Guid>("userid").ToString(),
                row.GetColumn<int>("productid"),
                row.GetColumn<int>("storelocationid"),
                row.GetColumn<int>("quantity"),
                row.GetColumn<decimal>("price") // TODO: decimal type
            ));

        return result;
    }

    public async Task<List<CartProductEntity>> GetUserCartProductsAsync(string userId)
    {
        var query = @"
                SELECT cartProducts.userid, cartProducts.productid, cartProducts.storelocationid, cartProducts.quantity 
                FROM cartProducts 
                WHERE userId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(userId))
        ];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        var result = new List<CartProductEntity>();

        foreach (Dictionary<string, object> row in rows)
            result.Add(new CartProductEntity(
                row.GetColumn<Guid>("userid").ToString(),
                row.GetColumn<int>("productid"),
                row.GetColumn<int>("storelocationid"),
                row.GetColumn<int>("quantity")
            ));

        return result;
    }

    public async Task RemoveAllCartItemsAsync(Guid userId)
    {
        var query = @"
                DELETE FROM cartProducts WHERE userId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(userId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task RemoveItemAsync(string userId, int productId)
    {
        var query = @"
                DELETE FROM cartProducts WHERE userId = $1 AND productId = $2;
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(userId)),
            new(productId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task<CartProductEntity?> UpdateItemAsync(CartProductEntity cartProduct)
    {
        var query = @"
                UPDATE cartProducts
                SET
                    quantity = $1
                WHERE userId = $2 AND productId = $3 
            ";

        QueryParameter[] parameters =
        [
            new(cartProduct.Quantity),
            new(new Guid(cartProduct.UserId)),
            new(cartProduct.ProductId)
        ];


        await _postgresService.ExecuteScalarAsync(query, parameters);

        return cartProduct;
    }
}