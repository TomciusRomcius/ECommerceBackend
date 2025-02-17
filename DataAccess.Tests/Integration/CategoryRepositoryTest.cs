using DataAccess.Test.Integration.Utils;
using ECommerce.DataAccess.Models.Category;
using ECommerce.DataAccess.Repositories;

namespace DataAccess.Tests.Integration
{
    public class CategoryRepositoryTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveCategory()
        {
            var testContainer = new TestDatabase();

            CategoryRepository categoryRepository = new CategoryRepository(testContainer._postgresService);
            string name = "category name";

            // Create category
            CategoryModel? category = await categoryRepository.CreateAsync(name);

            Assert.NotNull(category);
            Assert.Equal(name, category.Name);

            // Find category
            CategoryModel? retrieved = await categoryRepository.FindByNameAsync(name);

            Assert.NotNull(retrieved);
            Assert.Equal(category.Name, retrieved.Name);
            Assert.Equal(category.CategoryId, retrieved.CategoryId);

            await testContainer.DisposeAsync();
        }
    }
}