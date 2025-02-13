using ECommerce.Cart;
using ECommerce.DataAccess.Entities.PaymentSession;
using ECommerce.DataAccess.Repositories.PaymentSession;
using ECommerce.PaymentSession;
using Stripe;

namespace ECommerce.Order
{
    public class OrderService : IOrderService
    {
        readonly IPaymentSessionRepository _paymentSessionRepository;
        readonly ICartService _cartService;
        readonly IStripeSessionService _stripeSessionService;
        readonly ILogger _logger;

        public OrderService(IPaymentSessionRepository paymentSessionRepository, ICartService cartService, ILogger logger, IStripeSessionService stripeSessionService)
        {
            _paymentSessionRepository = paymentSessionRepository;
            _cartService = cartService;
            _logger = logger;
            _stripeSessionService = stripeSessionService;
        }

        public async Task<PaymentIntent?> CreateOrderPaymentSession(Guid userId)
        {
            if (await _paymentSessionRepository.GetPaymentSession(userId) is not null)
            {
                // Throw
                throw new InvalidOperationException("Cannot create a payment session: there is an existing payment session!");
            }

            var items = await _cartService.GetAllUserItems(userId.ToString());

            if (items.Count == 0)
            {
                throw new InvalidOperationException("There must be at least one cart item to be able to create a payment session!!");
            }

            double finalPrice = 0;
            items.ForEach(item => finalPrice += item.Price * item.Quantity);

            PaymentIntent? intentSession = null;

            if (finalPrice > 0)
            {
                _logger.LogInformation("Testing charge webhook");

                intentSession = _stripeSessionService.GeneratePaymentSession(
                    new() { UserId = userId.ToString(), Price = (int)(finalPrice * 100.0) }
                );

                await _paymentSessionRepository.CreatePaymentSessionAsync(
                    new PaymentSessionEntity(intentSession.Id, userId, "stripe")
                );
            }

            return intentSession;
        }
        public async Task OnCharge(Guid userId)
        {
            await _cartService.WipeAsync(userId);
            await _paymentSessionRepository.DeletePaymentSession(userId);
        }
    }
}