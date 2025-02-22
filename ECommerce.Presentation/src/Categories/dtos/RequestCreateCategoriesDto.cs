using System.ComponentModel.DataAnnotations;

namespace ECommerce.Categories
{
    public class RequestCreateCategoryDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public required string Name { get; set; }
    }
}