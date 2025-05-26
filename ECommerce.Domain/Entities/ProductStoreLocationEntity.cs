using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class ProductStoreLocationEntity
{
    public ProductStoreLocationEntity(int storeLocationId, int productId, int stock)
    {
        StoreLocationId = storeLocationId;
        ProductId = productId;
        Stock = stock;
    }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid StoreLocationId!")]
    public int StoreLocationId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid ProductId!")]
    public int ProductId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be lower than 0!")]
    public int Stock { get; set; }
}