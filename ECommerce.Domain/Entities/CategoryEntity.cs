using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities.Category
{
    public class CategoryEntity
    {
        [Required(ErrorMessage = "UserId is required!")]
        public int CategoryId { get; set; }
        [Length(2, 50, ErrorMessage = "Category name length must be between 2 and 50 characters long")]
        public string Name { get; set; }

        public CategoryEntity(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}