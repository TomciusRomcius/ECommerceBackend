
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.src.Entities;

[PrimaryKey(nameof(UserId), nameof(ProductId), nameof(StoreLocationId))]
public class CartProductEntity
{
    public CartProductEntity(string userId, int productId, int storeLocationId, int quantity)
    {
        UserId = userId;
        ProductId = productId;
        StoreLocationId = storeLocationId;
        Quantity = quantity;
    }

    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    [ForeignKey(nameof(StoreLocation))]
    public int StoreLocationId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than one!")]
    public int Quantity { get; set; }

    public IdentityUser? User { get; set; }
    public ProductEntity? Product { get; set; }
    public StoreLocationEntity? StoreLocation { get; set; }
}