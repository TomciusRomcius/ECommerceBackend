namespace ECommerce.Domain.Models;

public class CartProductModel
{
    public CartProductModel(string userId, int productId, int storeLocationId, int quantity, decimal price)
    {
        UserId = userId;
        ProductId = productId;
        StoreLocationId = storeLocationId;
        Quantity = quantity;
        Price = price;
    }

    public string UserId { get; set; }
    public int ProductId { get; set; }
    public int StoreLocationId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}