using ProductService.Application.Services;
using ProductService.Domain.Entities;
using ProductService.Presentation.Controllers.Product.Dtos;

namespace ProductService.Presentation.Controllers.Product;

internal static class ProductResponseMapper
{
    public static ProductWithStoreDto ToDto(
        ProductEntity product,
        IReadOnlyDictionary<int, ProductStoreDetails> storeDetailsByProductId)
    {
        ProductWithStoreDto dto = new()
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ManufacturerId = product.ManufacturerId,
            CategoryId = product.CategoryId,
            Manufacturer = product.Manufacturer,
            Category = product.Category,
        };

        if (storeDetailsByProductId.TryGetValue(product.ProductId, out ProductStoreDetails? storeDetails))
        {
            dto.Store = new StoreDetailsDto
            {
                StoreLocationId = storeDetails.StoreLocationId,
                Stock = storeDetails.Stock,
                DisplayName = storeDetails.DisplayName,
                Address = storeDetails.Address,
            };
        }

        return dto;
    }

    public static List<ProductWithStoreDto> ToDtoList(
        IEnumerable<ProductEntity> products,
        IReadOnlyDictionary<int, ProductStoreDetails> storeDetailsByProductId)
    {
        return products.Select(product => ToDto(product, storeDetailsByProductId)).ToList();
    }
}
