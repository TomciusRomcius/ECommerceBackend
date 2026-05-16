using ECommerceBackend.Utils.Microservices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.StoreProducts;

[ApiController]
[Route("[controller]")]
public class StoreProductsController(
    ILogger<StoreProductsController> logger,
    HttpClient httpClient,
    IOptions<MicroserviceHosts> hosts) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsFromStore(int pageNumber = 0, int pageSize = 20)
    {
        var query = new QueryString()
            .Add("pageNumber", pageNumber.ToString())
            .Add("pageSize", pageSize.ToString());

        string productUrl = $"{hosts.Value.ProductServiceUrl}/product{query}";
        logger.LogDebug("Fetching products from {Url}", productUrl);

        HttpResponseMessage productResponse = await httpClient.GetAsync(productUrl);
        productResponse.EnsureSuccessStatusCode();

        return Ok(new { data = await productResponse.Content.ReadFromJsonAsync<object>() });
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var query = new QueryString().Add("ids", productId.ToString());
        string productUrl = $"{hosts.Value.ProductServiceUrl}/product/by-ids{query}";
        logger.LogDebug("Fetching product {ProductId} from {Url}", productId, productUrl);

        HttpResponseMessage productResponse = await httpClient.GetAsync(productUrl);
        productResponse.EnsureSuccessStatusCode();

        List<object>? products = await productResponse.Content.ReadFromJsonAsync<List<object>>();
        object? product = products?.FirstOrDefault();

        if (product is null)
        {
            return NotFound();
        }

        return Ok(new { data = product });
    }
}
