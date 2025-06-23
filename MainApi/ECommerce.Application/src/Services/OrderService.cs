using ECommerce.Application.src.Interfaces;
using ECommerce.Application.src.Utils;
using ECommerce.Domain.src.Enums;
using ECommerce.Domain.src.Interfaces.Services;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Services.Order;
using ECommerce.Domain.src.Utils;
using ECommerce.Persistence;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerce.Application.src.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly DatabaseContext _context;
    private readonly IPaymentSessionService _paymentSessionService;
    private readonly IMediator _mediator;
    private readonly IOrderPriceCalculator _orderPriceCalculator;
    // private readonly IOrderValidator _orderValidator;
    private readonly MicroserviceNetworkConfig _microserviceNetworkConfig;

    public OrderService(
        ILogger<OrderService> logger,
        IOptions<MicroserviceNetworkConfig> microserviceNetworkConfig,
        IPaymentSessionService paymentSessionService,
        IMediator mediator,
        // IOrderValidator orderValidator,
        IOrderPriceCalculator orderPriceCalculator, DatabaseContext context)
    {
        _logger = logger;
        _paymentSessionService = paymentSessionService;
        _microserviceNetworkConfig = microserviceNetworkConfig.Value;
        _mediator = mediator;
        // _orderValidator = orderValidator;
        _orderPriceCalculator = orderPriceCalculator;
        _context = context;
    }

    public async Task<PaymentSessionModel?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
    {
        _logger.LogTrace("Entered CreateOrderPaymentSession");
        _logger.LogDebug("Creating order payment session for user: {userId}", userId);

        // TODO: check if payment session already exists
        Result<List<CartProductModel>> itemsResult = await _mediator.Send(new GetUserCartItemsDetailedQuery(userId));
        if (itemsResult.Errors.Any())
        {
            // TODO: handle
            return null;
        }

        List<CartProductModel> items = itemsResult.GetValue();

        // TODO: could probably make this more efficient
        List<int> storeLocationIds = items.Select(i => i.StoreLocationId).ToList();
        List<int> productIds = items.Select(i => i.ProductId).ToList();

        _logger.LogTrace("Querying products from user cart");
        List<ProductStoreLocationEntity> products = await _context.ProductStoreLocations
            .Where(psl => storeLocationIds.Contains(psl.StoreLocationId) && productIds.Contains(psl.ProductId))
            .ToListAsync();

        _logger.LogDebug("Retrieved user products: {@Products}", products);

        // Throws an exception on error
        _orderValidator.Validate(items, products);

        decimal price = _orderPriceCalculator.CalculatePrice(items);

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
            return null;
        }
        else
        {
            _logger.LogDebug("Created payment session: {}", intentSession);
            _logger.LogTrace("Created payment session for user: {}", userId);
        }

        return intentSession.GetValue();
    }
}