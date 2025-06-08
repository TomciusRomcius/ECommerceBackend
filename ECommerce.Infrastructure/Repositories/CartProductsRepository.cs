using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using Npgsql;

namespace ECommerce.Infrastructure.Repositories;

public class CartProductsRepository : ICartProductsRepository
{
    private readonly IPostgresService _postgresService;

    public CartProductsRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task<ResultError?> AddItemAsync(CartProductEntity cartProduct)
    {
        ResultError? error = null;

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

        try
        {
            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
        catch (NpgsqlException ex)
        {
            if (ex.ErrorCode == int.Parse(PostgresErrorCodes.ForeignKeyViolation))
            {
                error = new ResultError(
                    ResultErrorType.VALIDATION_ERROR,
                    "Specified product or store location does not exist"
                );
            }
        }

        catch (Exception)
        {
            error = new ResultError(ResultErrorType.UNKNOWN_ERROR, "Unknown error.");
        }

        return error;
    }

    public async Task<Result<List<CartProductModel>>> GetUserCartProductsDetailedAsync(string userId)
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
        {
            result.Add(new CartProductModel(
                row.GetColumn<Guid>("userid").ToString(),
                row.GetColumn<int>("productid"),
                row.GetColumn<int>("storelocationid"),
                row.GetColumn<int>("quantity"),
                row.GetColumn<decimal>("price")
            ));
        }

        return new Result<List<CartProductModel>>(result);
    }

    public async Task<Result<List<CartProductEntity>>> GetUserCartProductsAsync(string userId)
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
        {
            result.Add(new CartProductEntity(
                row.GetColumn<Guid>("userid").ToString(),
                row.GetColumn<int>("productid"),
                row.GetColumn<int>("storelocationid"),
                row.GetColumn<int>("quantity")
            ));
        }

        return new Result<List<CartProductEntity>>(result);
    }

    public async Task<ResultError?> RemoveAllCartItemsAsync(Guid userId)
    {
        var query = @"
                DELETE FROM cartProducts WHERE userId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(userId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);

        return null;
    }

    public async Task<ResultError?> RemoveItemAsync(string userId, int productId)
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

        return null;
    }

    public async Task<ResultError?> UpdateItemAsync(CartProductEntity cartProduct)
    {
        ResultError? error = null;

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

        try
        {
            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        catch (Exception)
        {
            error = new ResultError(ResultErrorType.UNKNOWN_ERROR, "Unknown error.");
        }

        return error;
    }
}