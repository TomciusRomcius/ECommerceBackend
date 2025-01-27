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
            return Ok("Hello");
        }

        [HttpPost()]
        [Authorize()]
        public async Task<IActionResult> CreateProducts([FromBody()] RequestCreateProductsDto createProductsDto)
        {
            string[] res = await _productService.CreateProducts(createProductsDto);
            return Ok(res);
        }
    }
}