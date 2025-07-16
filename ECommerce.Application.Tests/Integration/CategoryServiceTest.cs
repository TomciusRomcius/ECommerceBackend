using ECommerce.Application.Tests.Utils;
using ECommerce.Domain.src.Entities;
using ECommerce.Persistence.src;
using ECommerce.Presentation.src.Controllers.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Tests.Integration
{
    public class CategoryServiceTest : DbContextWithDependencyInjection
    {
        public CategoryServiceTest() : base() { }

        protected override void PreServiceProviderCreation(IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
        }

        [Fact]
        public async Task CreateCategory_ShouldCreateACategory()
        {
            // Arrange
            using IServiceScope scope = ServiceProvider.CreateScope();
            ICategoriesService categoryService = ServiceProvider.GetRequiredService<ICategoriesService>();
            DatabaseContext dbContext = ServiceProvider.GetRequiredService<DatabaseContext>();

            string categoryName = "New category";
            var categoryEntity = new CategoryEntity(categoryName);
            await categoryService.CreateCategory(categoryEntity);

            // Act
            List<CategoryEntity> retrievedCategories = await dbContext.Categories.ToListAsync();

            // Assert
            Assert.Single(retrievedCategories);
            Assert.Equal(categoryName, retrievedCategories[0].Name);
            Assert.True(retrievedCategories[0].CategoryId >= 0);
        }

        [Fact]
        public async Task GetAllCategories_ShouldGetAllCategories()
        {
            // Arrange
            using IServiceScope scope = ServiceProvider.CreateScope();
            ICategoriesService categoryService = ServiceProvider.GetRequiredService<ICategoriesService>();
            DatabaseContext dbContext = ServiceProvider.GetRequiredService<DatabaseContext>();

            string[] categories = ["Category1", "Category2", "Category3"];
            await dbContext.Categories.AddRangeAsync(categories.Select(name => new CategoryEntity(name)));
            await dbContext.SaveChangesAsync();

            // Act
            List<CategoryEntity> retrievedCategories = await categoryService.GetAllCategories();

            // Assert
            Assert.Equal(categories.Length, retrievedCategories.Count);
            foreach (var category in retrievedCategories)
            {
                Assert.Contains(category.Name, categories);
            }
        }
    }
}
