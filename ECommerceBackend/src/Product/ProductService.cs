using ECommerce.Domain.Entities.Product;
using ECommerce.Domain.Repositories.Product;

namespace ECommerce.Product
{
    public interface IProductService
    {
        public Task<List<ProductEntity>> GetAllProducts();
        public Task<ProductEntity?> CreateProduct(RequestCreateProductDto createProductDto);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductEntity>> GetAllProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<ProductEntity?> CreateProduct(RequestCreateProductDto createProductDto)
        {
            ProductEntity? result = await _productRepository.CreateAsync(
                new ProductEntity(
                 createProductDto.Name,
                 createProductDto.Description,
                 createProductDto.Price,
                 createProductDto.ManufacturerId,
                 createProductDto.CategoryId
                )
            );

            return result;
        }
    }
}