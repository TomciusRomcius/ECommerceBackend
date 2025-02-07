namespace ECommerce.DataAccess.Entities.CartProduct
{
    public class CartProductEntity
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int StoreLocationId { get; set; }
        public int Quantity { get; set; }

        public CartProductEntity(string userId, int productId, int storeLocationId, int quantity)
        {
            UserId = userId;
            ProductId = productId;
            StoreLocationId = storeLocationId;
            Quantity = quantity;
        }
    }
}