namespace ECommerce.DataAccess.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public CategoryModel(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}