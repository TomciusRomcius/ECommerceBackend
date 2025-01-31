namespace ECommerce.DataAccess.Models.Category
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