namespace ECommerce.Domain.Entities.Category
{
    public class CategoryEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public CategoryEntity(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}