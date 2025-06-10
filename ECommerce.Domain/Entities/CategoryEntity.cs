using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class CategoryEntity
{
    public CategoryEntity(string name)
    {
        Name = name;
    }
    
    public CategoryEntity(int categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }

    [Required(ErrorMessage = "UserId is required!")]
    public int CategoryId { get; set; } = -1;

    [Length(2, 50, ErrorMessage = "Category name length must be between 2 and 50 characters long")]
    public string Name { get; set; }
}