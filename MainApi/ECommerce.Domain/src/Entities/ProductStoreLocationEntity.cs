using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.src.Entities;

public class ProductStoreLocationEntity
{
    public ProductStoreLocationEntity(int storeLocationId, int productId, int stock)
    {
        StoreLocationId = storeLocationId;
        ProductId = productId;
        Stock = stock;
    }

    [ForeignKey(nameof(StoreLocation))]
    public int StoreLocationId { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be lower than 0!")]
    public int Stock { get; set; }

    public StoreLocationEntity? StoreLocation { get; set; }
    public ProductEntity? Product { get; set; }
}