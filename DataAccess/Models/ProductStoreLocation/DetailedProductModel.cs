using ECommerce.DataAccess.Models.Product;

namespace ECommerce.DataAccess.Models.ProductStoreLocation
{
    public class DetailedProductModel : ProductModel
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