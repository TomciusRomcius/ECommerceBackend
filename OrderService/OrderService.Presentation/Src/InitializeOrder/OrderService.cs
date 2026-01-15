using OrderService.Application.Persistence;
using OrderService.Domain.Entities;
using OrderService.Payment;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public class OrderService : IOrderService
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderPriceCalculator _orderPriceCalculator;
    private readonly IPaymentSessionService _paymentSessionService;
    private readonly IUserCartService _userCartService;

    public OrderService(
        ILogger<OrderService> logger,
        IPaymentSessionService paymentSessionService,
        IOrderPriceCalculator orderPriceCalculator,
        IUserCartService userCartService,
        DatabaseContext dbContext)
    {
        _logger = logger;
        _paymentSessionService = paymentSessionService;
        _orderPriceCalculator = orderPriceCalculator;
        _userCartService = userCartService;
        _dbContext = dbContext;
    }

    /// <summary>
    ///     Fetches user's cart items, their prices and generates a payment session.
    /// </summary>
    public async Task<Result<PaymentSessionModel>> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
    {
        Guid orderId = Guid.NewGuid();
        
        _logger.LogTrace("Entered CreateOrderPaymentSession");
        _logger.LogDebug("Creating order payment session for user: {UserId}", userId);

        // TODO: validate order
        Result<IEnumerable<CartProductModel>> cartProductsResult =
            await _userCartService.GetUserCartProductModelsAsync(userId);
        if (cartProductsResult.Errors.Any())
        {
            return new Result<PaymentSessionModel>(cartProductsResult.Errors);
        }

        IEnumerable<CartProductModel> cartProducts = cartProductsResult.GetValue();
        
        decimal price = _orderPriceCalculator.CalculatePrice(cartProducts);

        Result<PaymentSessionModel> intentSession = await _paymentSessionService.GeneratePaymentSessionAsync(
            orderId,
            new GeneratePaymentSessionOptions
            {
                PaymentProvider = paymentProvider,
                PriceCents = Convert.ToInt32(price * 100), // Convert to cents
                UserId = userId.ToString()
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

        // Create new order and set active order to current order

        IEnumerable<OrderProductEntity> orderProducts = cartProducts.Select(cp => new OrderProductEntity
        {
            OrderId = orderId, ProductId = cp.ProductId, ProductName = "TODO", Quantity = cp.Quantity
        });

        OrderEntity order = new() { OrderEntityId = orderId, UserId = userId };

        _dbContext.OrderProducts.AddRange(orderProducts);
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        return new Result<PaymentSessionModel>(intentSession.GetValue());
    }
}
