using ECommerce.TestUtils.TestDatabase;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Repositories.PaymentSession;
using Microsoft.Extensions.Logging;
using Moq;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Entities.PaymentSession;

namespace DataAccess.Tests.Integration
{
    public class PaymentSessionRepositoryTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrievePaymentSession()
        {
            var testContainer = new TestDatabase();

            // Create test user
            var userRepository = new UserRepository(testContainer._postgresService, new Mock<ILogger>().Object);
            var userModel = new UserEntity(Guid.NewGuid().ToString(), "email@gmail.com", "passwordhash", "firstname", "lastname");
            await userRepository.CreateAsync(userModel);

            var paymentSessionRepository = new PaymentSessionRepository(testContainer._postgresService);

            // Create payment session
            PaymentSessionEntity? paymentSession = new("1234-1234-1234-1234", new Guid(userModel.UserId), "stripe");
            await paymentSessionRepository.CreatePaymentSessionAsync(paymentSession);

            // Retrieve user's pending payment session
            PaymentSessionEntity? retrieved = await paymentSessionRepository.GetPaymentSession(new Guid(userModel.UserId));

            Assert.NotNull(retrieved);
            Assert.Equal(paymentSession.PaymentSessionId, retrieved.PaymentSessionId);
            Assert.Equal(paymentSession.PaymentSessionProvider, retrieved.PaymentSessionProvider);
            Assert.Equal(paymentSession.UserId, retrieved.UserId);

            await testContainer.DisposeAsync();
        }
    }
}