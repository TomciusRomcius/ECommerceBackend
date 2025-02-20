using System.Data;
using ECommerce.Cart;
using ECommerce.DataAccess.Entities.PaymentSession;
using ECommerce.DataAccess.Models.ProductStoreLocation;
using ECommerce.DataAccess.Repositories.PaymentSession;
using ECommerce.DataAccess.Repositories.ProductStoreLocation;
using ECommerce.PaymentSession;
using ECommerce.ProductStoreLocation;
using Stripe;

namespace ECommerce.Order
{
    public class OrderService : IOrderService
    {
        readonly IPaymentSessionRepository _paymentSessionRepository;
        readonly ICartService _cartService;
        readonly IProductStoreLocationRepository _productStoreLocationRepository;
        readonly IStripeSessionService _stripeSessionService;
        readonly ILogger _logger;

        public OrderService(IPaymentSessionRepository paymentSessionRepository, ICartService cartService, ILogger logger, IStripeSessionService stripeSessionService, IProductStoreLocationRepository productStoreLocationRepository)
        {
            _paymentSessionRepository = paymentSessionRepository;
            _cartService = cartService;
            _logger = logger;
            _stripeSessionService = stripeSessionService;
            _productStoreLocationRepository = productStoreLocationRepository;
        }

        public async Task<PaymentIntent?> CreateOrderPaymentSession(Guid userId)
        {
            if (await _paymentSessionRepository.GetPaymentSession(userId) is not null)
            {
                throw new InvalidOperationException("Cannot create a payment session: there is an existing payment session!");
            }

            var items = await _cartService.GetAllUserItemsDetailed(userId.ToString());

            if (items.Count == 0)
            {
                throw new InvalidOperationException("There must be at least one cart item to be able to create a payment session!!");
            }

            // Check if cart items don't exceed product stock
            List<(int, int)> idTuple = items.Select((item) => (item.StoreLocationId, item.ProductId)).ToList();
            var products = await _productStoreLocationRepository.GetProductsFromStoreAsync(idTuple);

            foreach (var cartItem in items)
            {
                ProductStoreLocationModel? selected = products.Find((i) => i.StoreLocationId == cartItem.StoreLocationId && i.ProductId == cartItem.ProductId);
                if (selected is null)
                {
                    throw new DataException("Product doesn't exist");
                }

                int stock = selected.Stock;

                if (cartItem.Quantity > stock)
                {
                    throw new InvalidOperationException("Product quantity is larger than item stock");
                }
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
            var cartItems = await _cartService.GetAllUserItems(userId.ToString());
            await _productStoreLocationRepository.UpdateStock(cartItems);
            await _cartService.WipeAsync(userId);
            await _paymentSessionRepository.DeletePaymentSession(userId);
        }
    }
}