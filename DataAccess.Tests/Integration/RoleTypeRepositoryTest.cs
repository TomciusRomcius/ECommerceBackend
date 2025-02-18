using ECommerce.TestUtils.TestDatabase;
using ECommerce.DataAccess.Models.RoleType;
using ECommerce.DataAccess.Repositories;

namespace DataAccess.Tests.Integration
{
    public class RoleTypeRepositoryIntegrationTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveRoleType()
        {
            var testContainer = new TestDatabase();
            var roleTypeRepository = new RoleTypeRepository(testContainer._postgresService);

            string roleName = "Test role name";
            await roleTypeRepository.CreateAsync(new CreateRoleTypeModel(roleName));

            var retrieved = await roleTypeRepository.FindByNameAsync(roleName);

            Assert.NotNull(retrieved);
            Assert.Equal(roleName, retrieved.Name);

            await testContainer.DisposeAsync();
        }
    }
}