using ECommerce.TestUtils.TestDatabase;
using ECommerce.DataAccess.Entities.PaymentSession;
using ECommerce.DataAccess.Models.User;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Repositories.PaymentSession;
using Microsoft.Extensions.Logging;
using Moq;

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
            var userModel = new UserModel(new Guid().ToString(), "email@gmail.com", "passwordhash", "firstname", "lastname");
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