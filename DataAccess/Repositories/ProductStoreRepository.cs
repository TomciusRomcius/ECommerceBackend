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
                INSERT INTO productStoreLocation (storeLocationId, productId)
                VALUES ($1, $2);
            ";

            QueryParameter[] parameters = [
                new QueryParameter(model.StoreLocationId),
                new QueryParameter(model.ProductId)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task RemoveProductFromStore(string storeLocationId, string productId)
        {
            string query = @"
                DELETE FROM productStoreLocation WHERE storeLocationId = $1 AND productId = $2;
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
                UPDATE productStoreLocation 
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