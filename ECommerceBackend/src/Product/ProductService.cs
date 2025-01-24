using System.Text;
using ECommerce.Common.Services;
using ECommerce.Common.Utils;

namespace ECommerce.Product
{
    public interface IProductService
    {
        public Task GetAllProducts();
        public Task<string[]> CreateProducts(RequestCreateProductsDto createProductsDto);
    }

    public class ProductService : IProductService
    {
        private readonly IPostgresService _postgresService;
        private ILogger logger = LoggerManager.GetInstance().CreateLogger("ProductService");

        public ProductService(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public Task GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public async Task<string[]> CreateProducts(RequestCreateProductsDto createProductsDto)
        {
            StringBuilder query = new StringBuilder();
            List<QueryParameter> parameters = new List<QueryParameter>();

            int i = 0;
            foreach (ProductDto productDto in createProductsDto.Products)
            {
                string queryLine = @$"
                    INSERT INTO products(name, description, price, manufacturerId, categoryId) 
                    VALUES (@name{i}, @description{i}, @price{i}, @manufacturerId{i}, @categoryId{i})
                    RETURNING productId;
                ";

                query.AppendLine(queryLine);
                parameters.Add(new QueryParameter($"name{i}", productDto.Name));
                parameters.Add(new QueryParameter($"description{i}", productDto.Description));
                parameters.Add(new QueryParameter($"price{i}", productDto.Price));
                parameters.Add(new QueryParameter($"manufacturerId{i}", productDto.ManufacturerId));
                parameters.Add(new QueryParameter($"categoryId{i}", productDto.CategoryId));
            }

            List<Dictionary<string, object>> results = await _postgresService.ExecuteAsync(query.ToString(), parameters.ToArray());

            throw new NotImplementedException();
        }
    }
}