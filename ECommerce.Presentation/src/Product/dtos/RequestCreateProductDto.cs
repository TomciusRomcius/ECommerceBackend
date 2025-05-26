using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.Product.dtos;

public class RequestCreateProductDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
    public required string Name { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Description cannot be empty")]
    public required string Description { get; set; }

    public required decimal Price { get; set; }
    public required int ManufacturerId { get; set; }
    public required int CategoryId { get; set; }
}