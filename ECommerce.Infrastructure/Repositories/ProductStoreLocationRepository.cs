using System.Text;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Entities.ProductStoreLocation;
using ECommerce.Domain.Models.ProductStoreLocation;
using ECommerce.Domain.Repositories.ProductStoreLocation;

namespace ECommerce.Infrastructure.Repositories.ProductStoreLocation
{
    public class ProductStoreLocationRepository : IProductStoreLocationRepository
    {
        readonly IPostgresService _postgresService;

        public ProductStoreLocationRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task AddProductToStore(ProductStoreLocationEntity model)
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
                result.Add(row.GetColumn<int>("productid"));
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

        public async Task<List<ProductStoreLocationEntity>> GetProductsFromStoreAsync(List<(int, int)> storeLocationIdProductId)
        {
            if (storeLocationIdProductId.Count == 0) return [];

            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(@"
                SELECT * FROM productStoreLocations
                WHERE (storeLocationId = $1 AND productId = $2)
            ");

            List<QueryParameter> parameters = [
                new(storeLocationIdProductId[0].Item1),
                new(storeLocationIdProductId[0].Item2)
            ];

            int index = 1;
            for (int i = 0; i < storeLocationIdProductId.Count(); i++)
            {
                (int, int) entry = storeLocationIdProductId[i];

                queryBuilder.AppendLine($"OR (storeLocationId = ${index} AND productId = ${index + 1})");
                parameters.Add(new QueryParameter(entry.Item1));
                parameters.Add(new QueryParameter(entry.Item2));
                index++;
            }

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(queryBuilder.ToString(), parameters.ToArray());
            List<ProductStoreLocationEntity> result = new List<ProductStoreLocationEntity>();

            foreach (var row in rows)
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
            string query = @"
                DELETE FROM productStoreLocations WHERE storeLocationId = $1 AND productId = $2;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(storeLocationId),
                new QueryParameter(productId)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task UpdateProduct(ProductStoreLocationEntity model)
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

        public async Task UpdateStock(List<CartProductEntity> cartProducts)
        {
            if (cartProducts.Count == 0)
            {
                return;
            }

            StringBuilder queryValues = new StringBuilder();

            List<QueryParameter> parameters = new List<QueryParameter>();

            queryValues.Append("VALUES ");

            for (int i = 0; i < cartProducts.Count(); i++)
            {
                queryValues.Append($"(${(i * 3) + 1}, ${(i * 3) + 2}, ${(i * 3) + 3})");

                if (i != cartProducts.Count() - 1)
                {
                    queryValues.Append(",");
                }

                var entry = cartProducts[i];


                parameters.Add(new QueryParameter(entry.StoreLocationId));
                parameters.Add(new QueryParameter(entry.ProductId));
                parameters.Add(new QueryParameter(entry.Quantity));
            }

            string queryBuilder = @$"
                UPDATE productStoreLocations as a
                SET stock = GREATEST(0, stock - b.dStock)
                FROM (
                    {queryValues}
                ) AS b(storeLocationId, productId, dStock)
                WHERE a.storeLocationId = b.storeLocationId AND a.productId = b.productId;
            ";

            await _postgresService.ExecuteScalarAsync(queryBuilder, parameters.ToArray());
        }
    }
}