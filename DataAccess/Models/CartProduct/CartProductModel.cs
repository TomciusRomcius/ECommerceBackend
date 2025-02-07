namespace ECommerce.DataAccess.Models.CartProduct
{
    public class CartProductModel
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int StoreLocationId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public CartProductModel(string userId, int productId, int storeLocationId, int quantity, double price)
        {
            UserId = userId;
            ProductId = productId;
            StoreLocationId = storeLocationId;
            Quantity = quantity;
            Price = price;
        }
    }
}