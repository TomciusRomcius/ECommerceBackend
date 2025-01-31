using System.Text;
using ECommerce.Common.Utils;
using ECommerce.DataAccess.Models;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.Product
{
    public interface IProductService
    {
        public Task<List<ProductModel>> GetAllProducts();
        public Task<ProductModel?> CreateProduct(RequestCreateProductDto createProductDto);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<ProductModel?> CreateProduct(RequestCreateProductDto createProductDto)
        {
            ProductModel? result = await _productRepository.CreateAsync(
                new ProductModel(
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