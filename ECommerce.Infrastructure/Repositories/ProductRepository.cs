using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;
using ECommerce.DataAccess.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.Product;
using ECommerce.Domain.Models.Product;
using ECommerce.Domain.Repositories.Product;

namespace ECommerce.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly IPostgresService _postgresService;

        public ProductRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<ProductEntity?> CreateAsync(ProductEntity product)
        {
            string query = @"
                    INSERT INTO products(name, description, price, manufacturerId, categoryId) 
                    VALUES ($1, $2, $3, $4, $5)
                    RETURNING productId;
                ";

            QueryParameter[] parameters = [
                new QueryParameter(product.Name),
                new QueryParameter(product.Description),
                new QueryParameter(product.Price),
                new QueryParameter(product.ManufacturerId),
                new QueryParameter(product.CategoryId)
            ];

            ProductEntity? result = null;

            object? id = await _postgresService.ExecuteScalarAsync(query.ToString(), parameters.ToArray());

            if (id is int)
            {
                result = product;
                result.ProductId = Convert.ToInt32(id);
            }

            return result;
        }

        public async Task UpdateAsync(UpdateProductModel product)
        {
            string query = @"
                    UPDATE products
                    SET name = COALESCE($1, name)
                    SET description = COALESCE($2, description)
                    SET price = COALESCE($3, price)
                    SET manufacturerId = COALESCE($4, manufacturerId)
                    SET categoryId = COALESCE($5, categoryId)
                    WHERE productId = $6;
                ";

            QueryParameter[] parameters = [
                new QueryParameter(product.Name),
                new QueryParameter(product.Description),
                new QueryParameter(product.Price),
                new QueryParameter(product.ManufacturerId),
                new QueryParameter(product.CategoryId),
                new QueryParameter(product.ProductId),
            ];

            await _postgresService.ExecuteScalarAsync(query.ToString(), parameters.ToArray());
        }

        public async Task DeleteAsync(int productId)
        {
            string query = @"
                    DELETE FROM products WHERE productId = $1; 
                ";

            QueryParameter[] parameters = [
                new QueryParameter(productId)
            ];

            await _postgresService.ExecuteScalarAsync(query.ToString(), parameters.ToArray());
        }

        public Task<ProductEntity?> FindByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductEntity?> FindByNameAsync(string productName)
        {
            string query = @"
                SELECT * FROM products;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query.ToString());
            ProductEntity? result = null;

            var row = rows[0];

            if (row is not null)
            {
                result = new ProductEntity(
                    row.GetColumn<int>("productid"),
                    row.GetColumn<string>("name"),
                    row.GetColumn<string>("description"),
                    row.GetColumn<decimal>("price"), // TODO: decimal
                    row.GetColumn<int>("manufacturerid"),
                    row.GetColumn<int>("categoryid")
                );
            }

            return result;
        }

        public async Task<List<ProductEntity>> GetAll()
        {
            string query = @"
                SELECT * FROM products;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query.ToString());
            List<ProductEntity> result = new List<ProductEntity>();

            foreach (var row in rows)
            {
                result.Add(new ProductEntity(
                    row.GetColumn<int>("productid"),
                    row.GetColumn<string>("name"),
                    row.GetColumn<string>("description"),
                    row.GetColumn<decimal>("price"), // TODO: decimal
                    row.GetColumn<int>("manufacturerid"),
                    row.GetColumn<int>("categoryid")
                ));
            }

            return result;
        }

        public Task<List<ProductEntity>> GetAllInCategory(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}