using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class CartProductEntity
{
    public CartProductEntity(string userId, int productId, int storeLocationId, int quantity)
    {
        UserId = userId;
        ProductId = productId;
        StoreLocationId = storeLocationId;
        Quantity = quantity;
    }

    [Required(ErrorMessage = "UserId is required!")]
    public string UserId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid ProductId!")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid StoreLocationId!")]
    public int StoreLocationId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than one!")]
    public int Quantity { get; set; }
}