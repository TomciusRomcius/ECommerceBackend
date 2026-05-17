using ECommerceBackend.Utils.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BFF.StoreProducts;

[ApiController]
[Route("[controller]")]
public class StoreProductsController(
    IStoreProductsService storeProductsService,
    ILogger<StoreProductsController> logger) : ControllerBase
{
    /// <summary>
    /// Returns products merged with store details from StoreService.
    /// Fetches the product page from ProductService, then enriches via productstorelocation/by-product-ids.
    /// When storeLocationId is set, only products stocked at that store are included.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int? storeLocationId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Page<StoreProductDto> page = await storeProductsService.GetProductsAsync(
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

    [HttpGet("{productId:int}")]
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
