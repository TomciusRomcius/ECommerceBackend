namespace ECommerce.DataAccess.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }

        public ProductModel(string name, string description, double price, int manufacturerId, int categoryId)
        {
            ProductId = -1;
            Name = name;
            Description = description;
            Price = price;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
        }

        public ProductModel(int productId, string name, string description, double price, int manufacturerId, int categoryId)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
        }
    }
}