namespace ECommerce.Categories
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public CategoryModel(int id, string name)
        {
            CategoryId = id;
            Name = name;
        }
    }
}