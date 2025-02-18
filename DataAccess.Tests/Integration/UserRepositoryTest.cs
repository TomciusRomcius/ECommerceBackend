using ECommerce.TestUtils.TestDatabase;
using ECommerce.DataAccess.Models.User;
using ECommerce.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace DataAccess.Tests.Integration
{
    public class UserRepositoryIntegrationTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveTheUser()
        {
            IUserRepository userRepository;
            var testContainer = new TestDatabase();
            userRepository = new UserRepository(testContainer._postgresService, new Mock<ILogger>().Object);

            var id = new Guid();
            await userRepository.CreateAsync(new UserModel(id.ToString(), "email@gmail.com", "passwordhash", "firstname", "lastname"));

            var retrieved = await userRepository.FindByEmailAsync("email@gmail.com");

            Assert.NotNull(retrieved);
            Assert.Equal("email@gmail.com", retrieved.Email);

            await testContainer.DisposeAsync();
        }
    }
}