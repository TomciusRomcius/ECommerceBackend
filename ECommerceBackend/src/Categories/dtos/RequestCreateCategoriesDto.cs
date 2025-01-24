using System.ComponentModel.DataAnnotations;

namespace ECommerce.Categories
{
    public class CategoryDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public required string Name { get; set; }
    }

    public class RequestCreateCategoriesDto
    {
        [Required()]
        public required CategoryDto[] Categories { get; set; }
    }
}