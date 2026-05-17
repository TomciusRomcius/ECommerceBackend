using System.Net.Http.Json;
using System.Text.Json;
using ECommerceBackend.Utils.Microservices;
using ECommerceBackend.Utils.Pagination;
using Microsoft.Extensions.Options;

namespace BFF.StoreProducts;

public class StoreProductsService(
    HttpClient httpClient,
    IOptions<MicroserviceHosts> hosts,
    ILogger<StoreProductsService> logger) : IStoreProductsService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Page<StoreProductDto>> GetProductsAsync(
        int? storeLocationId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (storeLocationId is null)
        {
            return await GetAllProductsAsync(pageNumber, pageSize, cancellationToken);
        }

        return await GetProductsFromStoreAsync(storeLocationId.Value, pageNumber, pageSize, cancellationToken);
    }

    public async Task<Page<StoreProductDto>> GetAllProductsAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryString()
            .Add("pageNumber", pageNumber.ToString())
            .Add("pageSize", pageSize.ToString());

        string productsUrl = $"{hosts.Value.ProductServiceUrl}/product{query}";
        logger.LogDebug("Fetching all products from {Url}", productsUrl);

        using HttpResponseMessage productsResponse = await httpClient.GetAsync(productsUrl, cancellationToken);
        productsResponse.EnsureSuccessStatusCode();

        Page<ProductFromServiceDto>? productPage =
            await productsResponse.Content.ReadFromJsonAsync<Page<ProductFromServiceDto>>(JsonOptions, cancellationToken);

        if (productPage is null)
        {
            return new Page<StoreProductDto>
            {
                Data = [],
                TotalCount = 0,
                HasNextPage = false,
                HasPrevPage = false,
            };
        }

        return new Page<StoreProductDto>
        {
            Data = productPage.Data.Select(ToStoreProductDto).ToList(),
            TotalCount = productPage.TotalCount,
            HasNextPage = productPage.HasNextPage,
            HasPrevPage = productPage.HasPrevPage,
        };
    }

    public async Task<Page<StoreProductDto>> GetProductsFromStoreAsync(
        int storeLocationId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var storeQuery = new QueryString()
            .Add("storeLocationId", storeLocationId.ToString())
            .Add("pageNumber", pageNumber.ToString())
            .Add("pageSize", pageSize.ToString());

        string storeUrl = $"{hosts.Value.StoreServiceUrl}/productstorelocation{storeQuery}";
        logger.LogDebug("Fetching store products from {Url}", storeUrl);

        using HttpResponseMessage storeResponse = await httpClient.GetAsync(storeUrl, cancellationToken);
        storeResponse.EnsureSuccessStatusCode();

        Page<ProductStoreLocationDto>? storePage =
            await storeResponse.Content.ReadFromJsonAsync<Page<ProductStoreLocationDto>>(JsonOptions, cancellationToken);

        if (storePage is null || storePage.Data.Count == 0)
        {
            return new Page<StoreProductDto>
            {
                Data = [],
                TotalCount = storePage?.TotalCount ?? 0,
                HasNextPage = storePage?.HasNextPage ?? false,
                HasPrevPage = storePage?.HasPrevPage ?? false,
            };
        }

        var productQuery = new QueryString();
        foreach (int productId in storePage.Data.Select(location => location.ProductId))
        {
            productQuery = productQuery.Add("ids", productId.ToString());
        }

        string productsUrl = $"{hosts.Value.ProductServiceUrl}/product/by-ids{productQuery}";
        logger.LogDebug("Fetching product details from {Url}", productsUrl);

        using HttpResponseMessage productsResponse = await httpClient.GetAsync(productsUrl, cancellationToken);
        productsResponse.EnsureSuccessStatusCode();

        List<ProductFromServiceDto>? products =
            await productsResponse.Content.ReadFromJsonAsync<List<ProductFromServiceDto>>(JsonOptions, cancellationToken);

        Dictionary<int, ProductFromServiceDto> productsById = (products ?? [])
            .ToDictionary(product => product.ProductId);

        List<StoreProductDto> mergedProducts = storePage.Data
            .Where(location => productsById.ContainsKey(location.ProductId))
            .Select(location => ToStoreProductDto(productsById[location.ProductId], location))
            .ToList();

        return new Page<StoreProductDto>
        {
            Data = mergedProducts,
            TotalCount = storePage.TotalCount,
            HasNextPage = storePage.HasNextPage,
            HasPrevPage = storePage.HasPrevPage,
        };
    }

    public async Task<StoreProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var query = new QueryString().Add("ids", productId.ToString());
        string productUrl = $"{hosts.Value.ProductServiceUrl}/product/by-ids{query}";
        logger.LogDebug("Fetching product {ProductId} from {Url}", productId, productUrl);

        using HttpResponseMessage productResponse = await httpClient.GetAsync(productUrl, cancellationToken);
        productResponse.EnsureSuccessStatusCode();

        List<ProductFromServiceDto>? products =
            await productResponse.Content.ReadFromJsonAsync<List<ProductFromServiceDto>>(JsonOptions, cancellationToken);

        ProductFromServiceDto? product = products?.FirstOrDefault();
        if (product is null)
        {
            return null;
        }

        return ToStoreProductDto(product);
    }

    private static StoreProductDto ToStoreProductDto(ProductFromServiceDto product)
    {
        return new StoreProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ManufacturerId = product.ManufacturerId,
            CategoryId = product.CategoryId,
            Manufacturer = product.Manufacturer,
            Category = product.Category,
            Store = product.Store is null
                ? null
                : new StoreProductStoreDto
                {
                    StoreLocationId = product.Store.StoreLocationId,
                    Stock = product.Store.Stock,
                    DisplayName = product.Store.DisplayName,
                    Address = product.Store.Address,
                },
        };
    }

    private static StoreProductDto ToStoreProductDto(
        ProductFromServiceDto product,
        ProductStoreLocationDto location)
    {
        return new StoreProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ManufacturerId = product.ManufacturerId,
            CategoryId = product.CategoryId,
            Manufacturer = product.Manufacturer,
            Category = product.Category,
            Store = new StoreProductStoreDto
            {
                StoreLocationId = location.StoreLocationId,
                Stock = location.Stock,
                DisplayName = location.DisplayName,
                Address = location.Address,
            },
        };
    }

    private sealed class ProductStoreLocationDto
    {
        public int ProductId { get; set; }

        public int StoreLocationId { get; set; }

        public int Stock { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }

    private sealed class ProductFromServiceDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int ManufacturerId { get; set; }

        public int CategoryId { get; set; }

        public JsonElement? Manufacturer { get; set; }

        public JsonElement? Category { get; set; }

        public ProductStoreDetailsFromServiceDto? Store { get; set; }
    }

    private sealed class ProductStoreDetailsFromServiceDto
    {
        public int StoreLocationId { get; set; }

        public int Stock { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
