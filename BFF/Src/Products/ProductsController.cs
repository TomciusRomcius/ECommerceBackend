using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Transfer;
using BFF.Configuration;
using BFF.Utils;
using ECommerceBackend.Utils.Microservices;
using ECommerceBackend.Utils.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.Products;

[ApiController]
[Route("[controller]")]
public class ProductsController(
    HttpClient httpClient,
    IOptions<MicroserviceHosts> hosts,
    IOptions<S3Configuration> s3Configuration,
    IAmazonS3 s3Client,
    IS3ImageUrlBuilder s3ImageUrlBuilder,
    ILogger<ProductsController> logger) : ControllerBase
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryString()
            .Add("pageNumber", pageNumber.ToString())
            .Add("pageSize", pageSize.ToString());
        string upstreamUrl = $"{hosts.Value.ProductServiceUrl}/Product{query}";

        using HttpResponseMessage response = await httpClient.GetAsync(upstreamUrl, cancellationToken);
        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return HttpResponseUtils.FromStringBody((int)response.StatusCode, body);
        }

        Page<ProductWithImageKeysDto>? page =
            JsonSerializer.Deserialize<Page<ProductWithImageKeysDto>>(body, JsonOptions);

        if (page is null)
        {
            return StatusCode(StatusCodes.Status502BadGateway);
        }

        Page<ProductWithImageUrlsDto> mapped = new()
        {
            Data = page.Data.Select(ToProductWithImageUrls).ToList(),
            TotalCount = page.TotalCount,
            HasNextPage = page.HasNextPage,
            HasPrevPage = page.HasPrevPage,
        };

        return Ok(mapped);
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetProduct(int productId, CancellationToken cancellationToken = default)
    {
        var query = new QueryString().Add("ids", productId.ToString());
        string upstreamUrl = $"{hosts.Value.ProductServiceUrl}/Product/by-ids{query}";

        using HttpResponseMessage response = await httpClient.GetAsync(upstreamUrl, cancellationToken);
        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return HttpResponseUtils.FromStringBody((int)response.StatusCode, body);
        }

        List<ProductWithImageKeysDto>? products =
            JsonSerializer.Deserialize<List<ProductWithImageKeysDto>>(body, JsonOptions);

        ProductWithImageKeysDto? product = products?.FirstOrDefault();
        if (product is null)
        {
            return NotFound();
        }

        return Ok(ToProductWithImageUrls(product));
    }

    private ProductWithImageUrlsDto ToProductWithImageUrls(ProductWithImageKeysDto product) =>
        new()
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ManufacturerId = product.ManufacturerId,
            CategoryId = product.CategoryId,
            Manufacturer = product.Manufacturer,
            Category = product.Category,
            ImageUrls = s3ImageUrlBuilder.BuildUrls(product.ImageKeys),
        };

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct(
        [FromForm] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        string upstreamUrl = $"{hosts.Value.ProductServiceUrl}/Product";
        logger.LogDebug("Creating product at {Url}", upstreamUrl);

        using var upstreamRequest = new HttpRequestMessage(HttpMethod.Post, upstreamUrl);
        string authorizationHeader = Request.Headers.Authorization.ToString();
        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            upstreamRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(authorizationHeader);
        }

        List<string> keys = [];
        foreach (IFormFile file in request.Files)
        {
            using var fileStream = new MemoryStream();
            await file.CopyToAsync(fileStream, cancellationToken);
            // TODO: use productId with prefix instead of guid
            string key = Guid.NewGuid().ToString();
            keys.Add(key);
            var req = new TransferUtilityUploadRequest()
            {
                BucketName = s3Configuration.Value.BucketName,
                Key = key,
                InputStream = fileStream
            };
            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync(req, cancellationToken);
        }

        upstreamRequest.Content = JsonContent.Create(new
        {
            name = request.Name,
            description = request.Description,
            price = request.Price,
            manufacturerId = request.ManufacturerId,
            categoryId = request.CategoryId,
            imageKeys = keys,
        });

        using HttpResponseMessage response = await httpClient.SendAsync(upstreamRequest, cancellationToken);
        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning(
                "Create product failed with status {StatusCode}: {Body}",
                response.StatusCode,
                body);
        }

        return HttpResponseUtils.FromStringBody((int)response.StatusCode, body);
    }
}
