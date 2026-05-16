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

        if (psls is null || psls.Count == 0)
        {
            return Ok(new { data = Array.Empty<object>() });
        }

        var query = new QueryString();
        foreach (ProductStoreLocation psl in psls)
        {
            query = query.Add("ids", psl.ProductId.ToString());
        }

        string productDetailsUrl = $"{hosts.Value.ProductServiceUrl}/product/by-ids{query}";
        HttpResponseMessage productDetails = await httpClient.GetAsync(productDetailsUrl);
        productDetails.EnsureSuccessStatusCode();

        return Ok(new { data = await productDetails.Content.ReadFromJsonAsync<object>() });
    }
}
