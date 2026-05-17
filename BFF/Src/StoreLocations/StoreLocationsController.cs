using ECommerceBackend.Utils.Microservices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.StoreLocations;

public class StoreLocationDto
{
    public int StoreLocationId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

[ApiController]
[Route("[controller]")]
public class StoreLocationsController(ILogger<StoreLocationsController> logger, HttpClient httpClient, IOptions<MicroserviceHosts> hosts) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStoreLocations([FromQuery] int page)
    {
        var upstreamUrl = $"{hosts.Value.StoreServiceUrl}/StoreLocation?pageNumber={page}";
        logger.LogDebug("Fetching store locations from {Url}", upstreamUrl);

        var response = await httpClient.GetAsync(upstreamUrl);
        response.EnsureSuccessStatusCode();

        var locations = await response.Content.ReadFromJsonAsync<List<StoreLocationDto>>();
        return Ok(new { data = locations });
    }
}

