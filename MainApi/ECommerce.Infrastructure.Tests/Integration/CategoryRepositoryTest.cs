using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Tests.Utils;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration;

public class CategoryRepositoryTest
{
    [Fact]
    public async Task ShouldSuccesfullyCreateAndRetrieveCategory()
    {
        var testContainer = new TestDatabase();

        var categoryRepository = RepositoryFactories.CreateCategoryRepository(testContainer._postgresService);
        var name = "category name";

        // Create category
        Result<int> categoryResult = await categoryRepository.CreateAsync(name);

        // Find category
        CategoryEntity? retrieved = await categoryRepository.FindByNameAsync(name);
        
        Assert.Empty(categoryResult.Errors);
        Assert.NotNull(retrieved);
        Assert.Equal(name, retrieved.Name);
        Assert.Equal(categoryResult.GetValue(), retrieved.CategoryId);

        await testContainer.DisposeAsync();
    }
}