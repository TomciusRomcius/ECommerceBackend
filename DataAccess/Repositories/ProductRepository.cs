using ECommerce.DataAccess.Models.Product;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly IPostgresService _postgresService;

        public ProductRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<ProductModel?> CreateAsync(ProductModel product)
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

            ProductModel? result = null;

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

        public Task<ProductModel?> FindByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel?> FindByNameAsync(string productName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductModel>> GetAll()
        {
            string query = @"
                SELECT * FROM products;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query.ToString());
            List<ProductModel> result = new List<ProductModel>();

            foreach (var row in rows)
            {
                // TODO: implement safe access
                result.Add(new ProductModel(
                    Convert.ToInt32(row["productid"]),
                    row["name"].ToString()!,
                    row["description"].ToString()!,
                    Convert.ToInt32(row["price"]),
                    Convert.ToInt32(row["manufacturerid"]),
                    Convert.ToInt32(row["categoryid"])
                ));
            }

            return result;
        }

        public Task<List<ProductModel>> GetAllInCategory(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}