namespace ECommerce.DataAccess.Models.ProductStoreLocation
{
    public class ProductStoreLocationModel
    {
        public int StoreLocationId { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }

        public ProductStoreLocationModel(int storeLocationId, int productId, int stock)
        {
            StoreLocationId = storeLocationId;
            ProductId = productId;
            Stock = stock;
        }
    }
}