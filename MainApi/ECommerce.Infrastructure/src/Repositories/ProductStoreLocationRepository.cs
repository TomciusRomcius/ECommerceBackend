using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using ECommerce.Infrastructure.src.Services;
using ECommerce.Infrastructure.src.Utils;
using Npgsql;
using System.Text;

namespace ECommerce.Infrastructure.src.Repositories;

public class ProductStoreLocationRepository : IProductStoreLocationRepository
{
    private readonly IPostgresService _postgresService;

    public ProductStoreLocationRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task<ResultError?> AddProductToStore(ProductStoreLocationEntity model)
    {
        var query = @"
                INSERT INTO ProductStoreLocations (storeLocationId, productId, stock)
                VALUES ($1, $2, $3);
            ";

        QueryParameter[] parameters =
        [
            new(model.StoreLocationId),
            new(model.ProductId),
            new(model.Stock)
        ];

        ResultError? error = null;

        try
        {
            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
        catch (NpgsqlException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                error = new ResultError(
                    ResultErrorType.INVALID_OPERATION_ERROR,
                    "Trying to add product to store location, when product or store location does not exist!"
                );
            }

            else if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                error = new ResultError(
                    ResultErrorType.INVALID_OPERATION_ERROR,
                    "Trying to add product to store location when it already is added!"
                );
            }

            else
            {
                error = new ResultError(
                    ResultErrorType.UNKNOWN_ERROR,
                    "Failed to add product to store location due to unknown reasons."
                );
            }
        }

        catch (Exception ex)
        {
            error = new ResultError(
                ResultErrorType.UNKNOWN_ERROR,
                "Failed to add product to store location due to unknown reasons."
            );
        }

        return error;
    }

    public async Task<List<int>> GetProductIdsFromStoreAsync(int storeLocationId)
    {
        var query = @"
                SELECT productId FROM ProductStoreLocations WHERE storeLocationId = $1;
            ";

        QueryParameter[] parameters = [new(storeLocationId)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        var result = new List<int>();

        foreach (Dictionary<string, object> row in rows) result.Add(row.GetColumn<int>("productid"));

        return result;
    }

    public async Task<List<DetailedProductModel>> GetProductsFromStoreAsync(int storeLocationId)
    {
        var query = @"
                SELECT * FROM ProductStoreLocations
                INNER JOIN Products
                ON ProductStoreLocations.productId = Products.productId
                WHERE storeLocationId = $1;
            ";

        QueryParameter[] parameters = [new(storeLocationId)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        var result = new List<DetailedProductModel>();

        foreach (Dictionary<string, object> row in rows)
        {
            var model = new DetailedProductModel(
                row.GetColumn<int>("productid"),
                row.GetColumn<string>("name"),
                row.GetColumn<string>("description"),
                row.GetColumn<decimal>("price"), // TODO: decimal
                row.GetColumn<int>("manufacturerid"),
                row.GetColumn<int>("categoryid"),
                row.GetColumn<int>("stock")
            );

            result.Add(model);
        }

        return result;
    }

    public async Task<List<ProductStoreLocationEntity>> GetProductsFromStoreAsync(
        List<(int, int)> storeLocationIdProductId)
    {
        if (storeLocationIdProductId.Count == 0) return [];

        var queryBuilder = new StringBuilder();
        queryBuilder.AppendLine(@"
                SELECT * FROM ProductStoreLocations
                WHERE (storeLocationId = $1 AND productId = $2)
            ");

        List<QueryParameter> parameters =
        [
            new(storeLocationIdProductId[0].Item1),
            new(storeLocationIdProductId[0].Item2)
        ];

        var index = 1;
        for (var i = 0; i < storeLocationIdProductId.Count(); i++)
        {
            (int, int) entry = storeLocationIdProductId[i];

            queryBuilder.AppendLine($"OR (storeLocationId = ${index} AND productId = ${index + 1})");
            parameters.Add(new QueryParameter(entry.Item1));
            parameters.Add(new QueryParameter(entry.Item2));
            index++;
        }

        List<Dictionary<string, object>> rows =
            await _postgresService.ExecuteAsync(queryBuilder.ToString(), parameters.ToArray());
        var result = new List<ProductStoreLocationEntity>();

        foreach (Dictionary<string, object> row in rows)
        {
            var model = new ProductStoreLocationEntity(
                row.GetColumn<int>("storelocationid"),
                row.GetColumn<int>("productid"),
                row.GetColumn<int>("stock")
            );

            result.Add(model);
        }

        return result;
    }

    public async Task RemoveProductFromStore(int storeLocationId, int productId)
    {
        var query = @"
                DELETE FROM ProductStoreLocations WHERE storeLocationId = $1 AND productId = $2;
            ";

        QueryParameter[] parameters =
        [
            new(storeLocationId),
            new(productId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task UpdateProduct(ProductStoreLocationEntity model)
    {
        var query = @"
                UPDATE ProductStoreLocations
                SET
                stock = COALESCE($1, stock)
                WHERE storeLocationId = $1 AND productId = $2;
            ";

        QueryParameter[] parameters =
        [
            new(model.Stock),
            new(model.StoreLocationId),
            new(model.ProductId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task UpdateStock(List<CartProductEntity> cartProducts)
    {
        if (cartProducts.Count == 0) return;

        var queryValues = new StringBuilder();

        var parameters = new List<QueryParameter>();

        queryValues.Append("VALUES ");

        for (var i = 0; i < cartProducts.Count(); i++)
        {
            queryValues.Append($"(${i * 3 + 1}, ${i * 3 + 2}, ${i * 3 + 3})");

            if (i != cartProducts.Count() - 1) queryValues.Append(",");

            CartProductEntity entry = cartProducts[i];

            parameters.Add(new QueryParameter(entry.StoreLocationId));
            parameters.Add(new QueryParameter(entry.ProductId));
            parameters.Add(new QueryParameter(entry.Quantity));
        }

        var queryBuilder = $"""
                UPDATE "ProductStoreLocations" as a
                SET "Stock" = GREATEST(0, "Stock" - b."DStock")
                FROM (
                    {queryValues}
                ) AS b("StoreLocationId", "ProductId", "DStock")
                WHERE a."StoreLocationId" = b."StoreLocationId" AND a."ProductId" = b."ProductId";
            """;

        await _postgresService.ExecuteScalarAsync(queryBuilder, parameters.ToArray());
    }
}