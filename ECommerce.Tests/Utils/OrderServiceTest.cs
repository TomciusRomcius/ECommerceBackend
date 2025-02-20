using ECommerce.Cart;
using ECommerce.DataAccess.Entities.PaymentSession;
using ECommerce.DataAccess.Models.CartProduct;
using ECommerce.DataAccess.Models.ProductStoreLocation;
using ECommerce.DataAccess.Repositories.PaymentSession;
using ECommerce.DataAccess.Repositories.ProductStoreLocation;
using ECommerce.Order;
using ECommerce.PaymentSession;
using Microsoft.Extensions.Logging;
using Moq;
using Stripe;
using Xunit;

namespace ECommerce.Tests.Unit
{
    public class OrderServiceTest
    {
        [Fact]
        public async Task CreateOrderPaymentSession_ShouldCreatePaymentSession()
        {
            var paymentSessionRepository = new Mock<IPaymentSessionRepository>();
            var cartService = new Mock<ICartService>();
            var loggerService = new Mock<ILogger>();
            var stripeSessionService = new Mock<IStripeSessionService>();
            var productStoreLocationRepository = new Mock<IProductStoreLocationRepository>();

            var orderService = new OrderService(
                paymentSessionRepository.Object,
                cartService.Object,
                loggerService.Object,
                stripeSessionService.Object,
                productStoreLocationRepository.Object
            );

            Guid userId = Guid.NewGuid();

            stripeSessionService.Setup((service) => service.GeneratePaymentSession(It.IsAny<GeneratePaymentSessionOptions>()))
                .Returns(new PaymentIntent() { Id = "id" });

            paymentSessionRepository.Setup((repository) => repository.CreatePaymentSessionAsync(It.IsAny<PaymentSessionEntity>()));
            paymentSessionRepository.Setup((repository) => repository.GetPaymentSession(It.IsAny<Guid>()))
                .ReturnsAsync((PaymentSessionEntity?)null);

            cartService.Setup((service) => service.GetAllUserItemsDetailed(It.IsAny<string>())).ReturnsAsync(
                new List<CartProductModel>
                {
                    new(userId.ToString(), 1, 1, 5, 5.99),
                    new(userId.ToString(), 2, 1, 7, 5.99),
                }
            );

            productStoreLocationRepository.Setup((repository) => repository.GetProductsFromStoreAsync(It.IsAny<List<(int, int)>>()))
                .ReturnsAsync(new List<ProductStoreLocationModel>
                {
                    new (1, 1, 5),
                    new (1, 2, 9)
                });

            PaymentIntent? paymentIntent = await orderService.CreateOrderPaymentSession(userId);

            Assert.NotNull(paymentIntent);
        }

        [Fact]
        public async Task CreateOrderPaymentSession_ShouldThrowInvalidOperation_WhenQuantityIsLargerThanStock()
        {
            var paymentSessionRepository = new Mock<IPaymentSessionRepository>();
            var cartService = new Mock<ICartService>();
            var loggerService = new Mock<ILogger>();
            var stripeSessionService = new Mock<IStripeSessionService>();
            var productStoreLocationRepository = new Mock<IProductStoreLocationRepository>();

            var orderService = new OrderService(
                paymentSessionRepository.Object,
                cartService.Object,
                loggerService.Object,
                stripeSessionService.Object,
                productStoreLocationRepository.Object
            );

            Guid userId = Guid.NewGuid();

            stripeSessionService.Setup((service) => service.GeneratePaymentSession(It.IsAny<GeneratePaymentSessionOptions>()))
                .Returns(new PaymentIntent() { Id = "id" });

            paymentSessionRepository.Setup((repository) => repository.CreatePaymentSessionAsync(It.IsAny<PaymentSessionEntity>())).Returns(Task.CompletedTask);
            paymentSessionRepository.Setup((repository) => repository.GetPaymentSession(It.IsAny<Guid>())).ReturnsAsync((PaymentSessionEntity?)null);

            cartService.Setup((service) => service.GetAllUserItemsDetailed(It.IsAny<string>())).ReturnsAsync(
                new List<CartProductModel>
                    {
                    new (userId.ToString(), 1, 1, 5, 5.99),
                    new (userId.ToString(), 2, 1, 7, 5.99)
                    }
                );

            productStoreLocationRepository.Setup((repository) => repository.GetProductsFromStoreAsync(It.IsAny<List<(int, int)>>()))
                .ReturnsAsync(new List<ProductStoreLocationModel>
                {
                    new (1, 1, 5),
                    new (1, 2, 5), // Lower stock than desired quantity
                });

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await orderService.CreateOrderPaymentSession(userId));
        }
    }
}