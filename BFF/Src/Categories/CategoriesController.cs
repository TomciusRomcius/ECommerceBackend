using System.Net.Http.Headers;
using System.Net.Http.Json;
using ECommerceBackend.Utils.Microservices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.Categories;

[ApiController]
[Route("[controller]")]
public class CategoriesController(
    HttpClient httpClient,
    IOptions<MicroserviceHosts> hosts,
    ILogger<CategoriesController> logger) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        string upstreamUrl = $"{hosts.Value.ProductServiceUrl}/Categories";
        logger.LogDebug("Creating category at {Url}", upstreamUrl);

        using var upstreamRequest = new HttpRequestMessage(HttpMethod.Post, upstreamUrl);
        string authorizationHeader = Request.Headers.Authorization.ToString();
        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            upstreamRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(authorizationHeader);
        }

        upstreamRequest.Content = JsonContent.Create(new { name = request.Name });

        using HttpResponseMessage response = await httpClient.SendAsync(upstreamRequest, cancellationToken);
        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning(
                "Create category failed with status {StatusCode}: {Body}",
                response.StatusCode,
                body);
        }

        return new ContentResult
        {
            StatusCode = (int)response.StatusCode,
            Content = body,
            ContentType = "application/json",
        };
    }
}
