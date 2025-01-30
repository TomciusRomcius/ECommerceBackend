namespace ECommerce.DataAccess.Models.CartProduct
{
    public class CartProductModel
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public CartProductModel(string userId, int productId, int quantity)
        {
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}