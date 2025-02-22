namespace ECommerce.DataAccess.Models.Product
{
    public class UpdateProductModel
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? ManufacturerId { get; set; }
        public int? CategoryId { get; set; }

        public UpdateProductModel(int productId, string? name, string? description, decimal? price, int? manufacturerId, int? categoryId)
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