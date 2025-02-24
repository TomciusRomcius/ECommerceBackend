using System.Data;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Application.UseCases.PaymentSession.Queries;
using ECommerce.Domain.Enums.PaymentProvider;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Repositories.ProductStoreLocation;
using ECommerce.Domain.Services;
using MediatR;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        IPaymentSessionFactory _paymentSessionFactory;
        IOrderValidator _orderValidator;
        IOrderPriceCalculator _orderPriceCalculator;
        ICartService _cartService;
        IProductStoreLocationRepository _productStoreLocationRepository;
        IMediator _mediator;

        public OrderService(IPaymentSessionFactory paymentSessionFactory, IProductStoreLocationRepository productStoreLocationRepository, IMediator mediator, IOrderValidator orderValidator, ICartService cartService, IOrderPriceCalculator orderPriceCalculator)
        {
            _paymentSessionFactory = paymentSessionFactory;
            _productStoreLocationRepository = productStoreLocationRepository;
            _mediator = mediator;
            _orderValidator = orderValidator;
            _cartService = cartService;
            _orderPriceCalculator = orderPriceCalculator;
        }

        public async Task<PaymentProviderSession?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
        {
            if (await _mediator.Send(new GetPaymentSessionQuery(userId)) is not null)
            {
                throw new InvalidOperationException("Cannot create a payment session: there is an existing payment session!");
            }

            var items = await _cartService.GetAllUserItemsDetailed(userId.ToString());
            List<(int, int)> idTuple = items.Select((item) => (item.StoreLocationId, item.ProductId)).ToList();
            var products = await _productStoreLocationRepository.GetProductsFromStoreAsync(idTuple);

            // Throws an exception on error
            _orderValidator.Validate(items, products);

            decimal price = _orderPriceCalculator.CalculatePrice(items);

            var paymentOptions = new GeneratePaymentSessionOptions
            {
                UserId = userId.ToString(),
                Price = (int)_orderPriceCalculator.CalculatePrice(items) * 100 // TODO: may be dangerous
            };

            var paymentSessionService = _paymentSessionFactory.CreatePaymentSessionService(paymentProvider);

            var intentSession = await paymentSessionService.GeneratePaymentSession(paymentOptions);

            // TODO: remove hard-coded provider
            await _mediator.Send(new CreatePaymentSessionCommand(userId, intentSession.SessionId, PaymentProvider.STRIPE));

            return intentSession;
        }
        public async Task OnCharge(Guid userId)
        {
            var cartItems = await _cartService.GetAllUserItems(userId.ToString());
            await _productStoreLocationRepository.UpdateStock(cartItems);
            await _cartService.WipeAsync(userId);
            await _mediator.Send(new DeletePaymentSessionCommand(userId));
        }
    }
}