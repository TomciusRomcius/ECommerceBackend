using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Repositories;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration.Repositories;

public class RoleTypeRepositoryIntegrationTest
{
    [Fact]
    public async Task ShouldSuccesfullyCreateAndRetrieveRoleType()
    {
        var testContainer = new TestDatabase();
        var roleTypeRepository = new RoleTypeRepository(testContainer._postgresService);

        var roleName = "Test role name";
        await roleTypeRepository.CreateAsync(new CreateRoleTypeModel(roleName));

        RoleTypeEntity? retrieved = await roleTypeRepository.FindByNameAsync(roleName);

        Assert.NotNull(retrieved);
        Assert.Equal(roleName, retrieved.Name);

        await testContainer.DisposeAsync();
    }
}