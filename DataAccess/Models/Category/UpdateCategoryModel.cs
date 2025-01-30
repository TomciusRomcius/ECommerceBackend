namespace ECommerce.DataAccess.Models
{
    public class UpdateCategoryModel
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }

        public UpdateCategoryModel(int categoryId, string? name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}