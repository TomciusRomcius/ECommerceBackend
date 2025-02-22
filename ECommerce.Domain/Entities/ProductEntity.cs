namespace ECommerce.Domain.Entities.Product
{
    public class ProductEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }

        public ProductEntity(string name, string description, decimal price, int manufacturerId, int categoryId)
        {
            ProductId = -1;
            Name = name;
            Description = description;
            Price = price;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
        }

        public ProductEntity(int productId, string name, string description, decimal price, int manufacturerId, int categoryId)
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