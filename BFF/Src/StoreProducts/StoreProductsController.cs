using System.Text;
using ECommerceBackend.Utils.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.StoreProducts;

public class ProductStoreLocation
{
    public int StoreLocationId { get; set; }

    public int ProductId { get; set; }

    public int Stock { get; set; }
}

[ApiController]
[Route("[controller]")]
public class StoreProductsController(ILogger<StoreProductsController> logger, HttpClient httpClient, IOptions<MicroserviceHosts> hosts) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsFromStore(int storeId, int page)
    {
        HttpResponseMessage productIds = await httpClient.GetAsync($"{hosts.Value.StoreServiceUrl}/productstorelocation");
        productIds.EnsureSuccessStatusCode();
        List<ProductStoreLocation>? psls = await productIds.Content.ReadFromJsonAsync<List<ProductStoreLocation>>();
        logger.LogDebug("Retrieved producat ids: {@Products}", psls);
        
        // Fetch product details
        var productDetailsUrlBuilder = new StringBuilder();
        productDetailsUrlBuilder.Append($"{hosts.Value.ProductServiceUrl}/product/");

        for (int i = 0; i < psls.Count(); i++)
        {
            if (i == 0)
            {
                productDetailsUrlBuilder.Append($"?ids={psls[i].ProductId}");
            }
            else
            {
                productDetailsUrlBuilder.Append($"&ids={psls[i].ProductId}");
            }
        }

        HttpResponseMessage productDetails = await httpClient.GetAsync(productDetailsUrlBuilder.ToString());
        return Ok(new { data = productDetails.Content.ReadFromJsonAsync<object>().Result });
    }
}
