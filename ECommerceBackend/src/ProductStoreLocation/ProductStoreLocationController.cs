using ECommerce.DataAccess.Models.ProductStoreLocation;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductStoreLocation
{
    [ApiController]
    [Route("[controller]")]
    public class ProductStoreLocationController : ControllerBase
    {
        readonly IProductStoreLocationService _productStoreLocationService;

        public ProductStoreLocationController(IProductStoreLocationService productStoreLocationService)
        {
            _productStoreLocationService = productStoreLocationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsFromStore([FromBody] GetProductsFromStoreDto getProductsFromStoreDto)
        {
            bool isDetailed = HttpContext.Request.Query["detailed"].FirstOrDefault() == "1";

            object result;
            if (isDetailed)
            {
                result = await _productStoreLocationService.GetProductsFromStore(getProductsFromStoreDto.StoreLocationId);
            }

            else
            {
                result = await _productStoreLocationService.GetProductIdsFromStore(getProductsFromStoreDto.StoreLocationId);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToStore([FromBody] AddProductToStoreDto addProductToStoreDto)
        {
            var model = new ProductStoreLocationModel(
                addProductToStoreDto.StoreLocationId,
                addProductToStoreDto.ProductId,
                addProductToStoreDto.Stock
            );

            await _productStoreLocationService.AddProductToStore(model);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProductFromStore([FromBody] RemoveProductFromStoreDto removeProductFromStoreDto)
        {
            await _productStoreLocationService.RemoveProductFromStore(removeProductFromStoreDto.StoreLocationId, removeProductFromStoreDto.ProductId);
            return Ok();
        }


        [HttpPatch]
        public async Task<IActionResult> ModifyProductFromStore([FromBody] AddProductToStoreDto addProductToStoreDto)
        {
            var model = new ProductStoreLocationModel(
                addProductToStoreDto.StoreLocationId,
                addProductToStoreDto.ProductId,
                addProductToStoreDto.Stock
            );

            await _productStoreLocationService.ModifyProductFromStore(model);
            return Ok();
        }
    }
}