using ECommerce.TestUtils.TestDatabase;
using ECommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using ECommerce.Domain.Repositories.User;
using ECommerce.Domain.Entities.User;

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
            await userRepository.CreateAsync(new UserEntity(id.ToString(), "email@gmail.com", "passwordhash", "firstname", "lastname"));

            var retrieved = await userRepository.FindByEmailAsync("email@gmail.com");

            Assert.NotNull(retrieved);
            Assert.Equal("email@gmail.com", retrieved.Email);

            await testContainer.DisposeAsync();
        }
    }
}