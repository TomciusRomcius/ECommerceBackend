using System.Net.Http.Headers;
using System.Net.Http.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Util.Internal;
using BFF.Utils;
using ECommerceBackend.Utils.Microservices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFF.Products;

[ApiController]
[Route("[controller]")]
public class ProductsController(
    HttpClient httpClient,
    IOptions<MicroserviceHosts> hosts,
    IAmazonS3 s3Client,
    ILogger<ProductsController> logger) : ControllerBase
{
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
                BucketName = "ecommerce-api",
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
