using Microsoft.AspNetCore.Mvc;

namespace BFF.StoreProducts;

[ApiController]
[Route("[controller]")]
public class StoreProductsController(
    IStoreProductsService storeProductsService,
    ILogger<StoreProductsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int? storeLocationId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await storeProductsService.GetProductsAsync(
                storeLocationId,
                pageNumber,
                pageSize,
                cancellationToken);
            return Ok(new { data = page });
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Failed to fetch products (storeLocationId={StoreLocationId}).", storeLocationId);
            return StatusCode(StatusCodes.Status502BadGateway, new { error = "Failed to fetch products." });
        }
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(int productId, CancellationToken cancellationToken = default)
    {
        try
        {
            StoreProductDto? product = await storeProductsService.GetProductByIdAsync(productId, cancellationToken);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(new { data = product });
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Failed to fetch product {ProductId}.", productId);
            return StatusCode(StatusCodes.Status502BadGateway, new { error = "Failed to fetch product." });
        }
    }
}
