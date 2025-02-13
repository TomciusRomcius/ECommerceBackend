using ECommerce.DataAccess.Models.ProductStoreLocation;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.DataAccess.Repositories.ProductStoreLocation
{
    public class ProductStoreLocationRepository : IProductStoreLocationRepository
    {
        readonly IPostgresService _postgresService;

        public ProductStoreLocationRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task AddProductToStore(ProductStoreLocationModel model)
        {
            string query = @"
                INSERT INTO productStoreLocations (storeLocationId, productId, stock)
                VALUES ($1, $2, $3);
            ";

            QueryParameter[] parameters = [
                new QueryParameter(model.StoreLocationId),
                new QueryParameter(model.ProductId),
                new QueryParameter(model.Stock)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task<List<int>> GetProductIdsFromStoreAsync(int storeLocationId)
        {
            string query = @"
                SELECT productId FROM productStoreLocations WHERE storeLocationId = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(storeLocationId)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            List<int> result = new List<int>();

            foreach (var row in rows)
            {
                // TODO: null safety
                result.Add(Convert.ToInt32(row["productid"]));
            }

            return result;
        }

        public async Task<List<DetailedProductModel>> GetProductsFromStoreAsync(int storeLocationId)
        {
            string query = @"
                SELECT * FROM productStoreLocations
                INNER JOIN products
                ON productStoreLocations.productId = products.productId
                WHERE storeLocationId = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(storeLocationId)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            List<DetailedProductModel> result = new List<DetailedProductModel>();

            foreach (var row in rows)
            {
                // TODO: null safety
                var model = new DetailedProductModel(
                    Convert.ToInt32(row["productid"]),
                    row["name"].ToString()!,
                    row["description"].ToString()!,
                    Convert.ToInt32(row["price"]),
                    Convert.ToInt32(row["manufacturerid"]),
                    Convert.ToInt32(row["categoryid"]),
                    Convert.ToInt32(row["stock"])
                );

                result.Add(model);
            }

            return result;
        }

        public async Task RemoveProductFromStore(int storeLocationId, int productId)
        {
            string query = @"
                DELETE FROM productStoreLocations WHERE storeLocationId = $1 AND productId = $2;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(storeLocationId),
                new QueryParameter(productId)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task UpdateProduct(ProductStoreLocationModel model)
        {
            string query = @"
                UPDATE productStoreLocations
                SET
                stock = COALESCE($1, stock)
                WHERE storeLocationId = $1 AND productId = $2;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(model.Stock),
                new QueryParameter(model.StoreLocationId),
                new QueryParameter(model.ProductId)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}