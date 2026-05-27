using System.Net.Http.Json;
using BFF.Utils;
using ECommerceBackend.Utils.Microservices;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{storeLocationId:int}")]
    public async Task<IActionResult> GetStoreLocation(
        int storeLocationId,
        CancellationToken cancellationToken = default)
    {
        var upstreamUrl = $"{hosts.Value.StoreServiceUrl}/StoreLocation/{storeLocationId}";
        logger.LogDebug("Fetching store location {StoreLocationId} from {Url}", storeLocationId, upstreamUrl);

        var response = await httpClient.GetAsync(upstreamUrl, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }

        response.EnsureSuccessStatusCode();

        var location = await response.Content.ReadFromJsonAsync<StoreLocationDto>(cancellationToken);
        if (location is null)
        {
            return NotFound();
        }

        return Ok(new { data = location });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStoreLocation(
        [FromBody] CreateStoreLocationRequest request,
        CancellationToken cancellationToken)
    {
        string upstreamUrl = $"{hosts.Value.StoreServiceUrl}/storelocation";

        using var upstreamRequest = new HttpRequestMessage(HttpMethod.Post, upstreamUrl);
        HttpRequestUtils.ApplyAuthorizationHeader(upstreamRequest, Request);

        upstreamRequest.Content = JsonContent.Create(new
        {
            displayName = request.DisplayName,
            address = request.Address,
        });

        using HttpResponseMessage response = await httpClient.SendAsync(upstreamRequest, cancellationToken);
        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning(
                "Create store location failed with status {StatusCode}: {Body}",
                response.StatusCode,
                body);
        }

        return HttpResponseUtils.FromStringBody((int)response.StatusCode, body);
    }
}

