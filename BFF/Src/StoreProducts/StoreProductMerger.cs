namespace BFF.StoreProducts;

internal static class StoreProductMerger
{
    public static List<StoreProductDto> Merge(
        IEnumerable<ProductFromServiceDto> products,
        IReadOnlyDictionary<int, ProductStoreLocationDto> storeDetailsByProductId,
        int? storeLocationId = null)
    {
        List<StoreProductDto> merged = [];

        foreach (ProductFromServiceDto product in products)
        {
            if (!storeDetailsByProductId.TryGetValue(product.ProductId, out ProductStoreLocationDto? storeDetails))
            {
                if (storeLocationId is not null)
                {
                    continue;
                }

                merged.Add(ToStoreProductDto(product, store: null));
                continue;
            }

            merged.Add(ToStoreProductDto(product, storeDetails));
        }

        return merged;
    }

    public static StoreProductDto ToStoreProductDto(
        ProductFromServiceDto product,
        ProductStoreLocationDto? store)
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
            Store = store is null
                ? null
                : new StoreProductStoreDto
                {
                    StoreLocationId = store.StoreLocationId,
                    Stock = store.Stock,
                    DisplayName = store.DisplayName,
                    Address = store.Address,
                },
        };
    }

    public static IReadOnlyDictionary<int, ProductStoreLocationDto> IndexStoreDetails(
        IEnumerable<ProductStoreLocationDto> storeDetails,
        int? storeLocationId = null)
    {
        IEnumerable<ProductStoreLocationDto> filtered = storeLocationId is null
            ? storeDetails
            : storeDetails.Where(detail => detail.StoreLocationId == storeLocationId);

        return filtered
            .GroupBy(detail => detail.ProductId)
            .ToDictionary(group => group.Key, group => group.First());
    }
}
