using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities.Product
{
    public class ProductEntity
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid ProductId")]
        public int ProductId { get; set; }
        [MinLength(5, ErrorMessage = "Invalid ProductId")]
        public string Name { get; set; }
        [MinLength(5, ErrorMessage = "Invalid ProductId")]
        public string Description { get; set; }
        [Range(0.1, double.MaxValue, ErrorMessage = "Invalid ProductId")]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Invalid ManufacturerId")]
        public int ManufacturerId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Invalid CategoryId")]
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