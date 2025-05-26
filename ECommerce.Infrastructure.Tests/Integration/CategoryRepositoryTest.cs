using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration;

public class CategoryRepositoryTest
{
    [Fact]
    public async Task ShouldSuccesfullyCreateAndRetrieveCategory()
    {
        var testContainer = new TestDatabase();

        var categoryRepository = new CategoryRepository(testContainer._postgresService);
        var name = "category name";

        // Create category
        CategoryEntity? category = await categoryRepository.CreateAsync(name);

        Assert.NotNull(category);
        Assert.Equal(name, category.Name);

        // Find category
        CategoryEntity? retrieved = await categoryRepository.FindByNameAsync(name);

        Assert.NotNull(retrieved);
        Assert.Equal(category.Name, retrieved.Name);
        Assert.Equal(category.CategoryId, retrieved.CategoryId);

        await testContainer.DisposeAsync();
    }
}