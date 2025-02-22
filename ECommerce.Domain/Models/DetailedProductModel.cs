using ECommerce.Domain.Entities.Product;

namespace ECommerce.Domain.Models.ProductStoreLocation
{
    public class DetailedProductModel : ProductEntity
    {
        public int Stock { get; set; }

        public DetailedProductModel(
            int productId,
            string name,
            string description,
            decimal price,
            int manufacturerId,
            int categoryId,
            int stock) : base(productId, name, description, price, manufacturerId, categoryId)
        {
            Stock = stock;
        }
    }
}