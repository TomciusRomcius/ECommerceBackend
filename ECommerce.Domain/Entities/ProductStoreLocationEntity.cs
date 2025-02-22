namespace ECommerce.Domain.Entities.ProductStoreLocation
{
    public class ProductStoreLocationEntity
    {
        public int StoreLocationId { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }

        public ProductStoreLocationEntity(int storeLocationId, int productId, int stock)
        {
            StoreLocationId = storeLocationId;
            ProductId = productId;
            Stock = stock;
        }
    }
}