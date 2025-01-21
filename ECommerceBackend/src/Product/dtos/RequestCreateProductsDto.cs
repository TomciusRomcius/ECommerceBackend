using System.ComponentModel.DataAnnotations;

namespace ECommerce.Product
{
    public class ProductDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public required string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description cannot be empty")]
        public required string Description { get; set; }
        public required double Price { get; set; }
        public required int ManufacturerId { get; set; }
        public required int CategoryId { get; set; }
    }
    public class RequestCreateProductsDto
    {
        [Required()]
        [MinLength(1, ErrorMessage = "You must create atleast one product")]
        public required ProductDto[] Products { get; set; }
    }
}