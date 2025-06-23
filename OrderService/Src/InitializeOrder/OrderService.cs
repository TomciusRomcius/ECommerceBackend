using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Payment;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IPaymentSessionService _paymentSessionService;
    private readonly IMediator _mediator;
    private readonly IOrderPriceCalculator _orderPriceCalculator;
    private readonly MicroserviceNetworkConfig _microserviceNetworkConfig;

    public OrderService(
        ILogger<OrderService> logger,
        IOptions<MicroserviceNetworkConfig> microserviceNetworkConfig,
        IPaymentSessionService paymentSessionService,
        IMediator mediator,
        // IOrderValidator orderValidator,
        IOrderPriceCalculator orderPriceCalculator)
    {
        _logger = logger;
        _paymentSessionService = paymentSessionService;
        _microserviceNetworkConfig = microserviceNetworkConfig.Value;
        _mediator = mediator;
        _orderPriceCalculator = orderPriceCalculator;
    }
    
    /// <summary>
    /// Fetches user's cart items, their prices and generates a payment session. 
    /// </summary>
    public async Task<Result<PaymentSessionModel>> CreateOrderPaymentSession(Guid userId, string jwtToken, PaymentProvider paymentProvider)
    {
        _logger.LogTrace("Entered CreateOrderPaymentSession");
        _logger.LogDebug("Creating order payment session for user: {UserId}", userId);

        // TODO: check if payment session already exists
        Result<List<CartProductMinimalModel>> userCartItemsResult = await _mediator.Send(new GetProductsFromUserCartQuery(userId, jwtToken));
        if (userCartItemsResult.Errors.Any())
        {
            // TODO: handle
            return new Result<PaymentSessionModel>([userCartItemsResult.Errors.First()]);
        }
        // Get cart products
        List<CartProductMinimalModel> userCartItems = userCartItemsResult.GetValue();

        // TODO: split into separate method and return ResultError on failure
        // Get cart product prices
        Result<List<ProductPriceModel>> productDetailsResult = await
            _mediator.Send(new GetProductDescriptionQuery(userCartItems.Select(i => i.ProductId).ToList()));

        if (productDetailsResult.Errors.Any())
        {
            _logger.LogError(
                "Failed to get product details of user cart items. User: {UserId} Errors: {@Errors}",
                userId,
                productDetailsResult.Errors
            );

            return new Result<PaymentSessionModel>(productDetailsResult.Errors);
        }
        
        List<ProductPriceModel> productPriceModels = productDetailsResult.GetValue();
        
        // Assign price values to cart products
        List<CartProductModel> cartProducts = new List<CartProductModel>();
        foreach (var cartPr in userCartItems)
        {
            var cartPricingModel = productPriceModels
                .FirstOrDefault(i => i.ProductId == cartPr.ProductId);

            if (cartPricingModel == null)
            {
                _logger.LogError(
                    "Trying to fetch a product from user cart, but the product '{ProductId}' does not exist!",
                    cartPr.ProductId
                );

                return new Result<PaymentSessionModel>([
                    new(ResultErrorType.VALIDATION_ERROR, "Product does not exist!")
                ]);
            }

            var cartProduct = new CartProductModel
            {
                ProductId = cartPr.ProductId,
                StoreLocationId = cartPr.StoreLocationId,
                Quantity = cartPr.Quantity,
                Price = cartPricingModel.Price
            };

            cartProducts.Add(cartProduct);
        }

        // TODO: validate order
        decimal price = _orderPriceCalculator.CalculatePrice(cartProducts);

        // Create payment session. TODO: extract to separate method
        var intentSession = await _paymentSessionService.GeneratePaymentSessionAsync(
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
        else
        {
            _logger.LogDebug("Created payment session: {}", intentSession);
            _logger.LogTrace("Created payment session for user: {}", userId);
        }

        return new Result<PaymentSessionModel>(intentSession.GetValue());
    }
}