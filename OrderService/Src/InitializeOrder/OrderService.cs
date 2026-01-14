using OrderService.Payment;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IUserCartService _userCartService;
    private readonly IOrderPriceCalculator _orderPriceCalculator;
    private readonly IPaymentSessionService _paymentSessionService;
    
    public OrderService(
        ILogger<OrderService> logger,
        IPaymentSessionService paymentSessionService,
        IOrderPriceCalculator orderPriceCalculator,
        IUserCartService userCartService)
    {
        _logger = logger;
        _paymentSessionService = paymentSessionService;
        _orderPriceCalculator = orderPriceCalculator;
        _userCartService = userCartService;
    }

    /// <summary>
    ///     Fetches user's cart items, their prices and generates a payment session. 
    /// </summary>
    public async Task<Result<PaymentSessionModel>> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
    {
        _logger.LogTrace("Entered CreateOrderPaymentSession");
        _logger.LogDebug("Creating order payment session for user: {UserId}", userId);
        
        // TODO: validate order
        Result<IEnumerable<CartProductModel>> cartProductsResult = await _userCartService.GetUserCartProductModelsAsync(userId);
        if (cartProductsResult.Errors.Any())
        {
            return new Result<PaymentSessionModel>(cartProductsResult.Errors);
        }
        
        IEnumerable<CartProductModel> cartProducts = cartProductsResult.GetValue();
        decimal price = _orderPriceCalculator.CalculatePrice(cartProducts);

        Result<PaymentSessionModel> intentSession = await _paymentSessionService.GeneratePaymentSessionAsync(
            new GeneratePaymentSessionOptions
            {
                PaymentProvider = paymentProvider,
                PriceCents = Convert.ToInt32(price * 100), // Convert to cents
                UserId = userId.ToString(),
            }
        );

        if (intentSession.Errors.Any())
        {
            // TODO: handle
            _logger.LogError("Failed to create payment intent for user: {}", userId);
            return new Result<PaymentSessionModel>([intentSession.Errors.First()]);
        }

        _logger.LogDebug("Created payment session: {}", intentSession);
        _logger.LogTrace("Created payment session for user: {}", userId);

        return new Result<PaymentSessionModel>(intentSession.GetValue());
    }
}