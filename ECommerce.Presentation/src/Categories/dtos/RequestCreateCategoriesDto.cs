using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.Categories.dtos;

public class RequestCreateCategoryDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
    public required string Name { get; set; }
}