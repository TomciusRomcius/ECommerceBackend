using ECommerce.DataAccess.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Product
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet()]
        [Authorize()]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }

        [HttpPost()]
        [Authorize()]
        public async Task<IActionResult> CreateProducts([FromBody()] RequestCreateProductDto createProductDto)
        {
            ProductModel? res = await _productService.CreateProduct(createProductDto);
            return Created(nameof(CreateProducts), res);
        }
    }
}