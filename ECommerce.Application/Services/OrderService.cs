using System.Data;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
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
        IProductStoreLocationRepository _productStoreLocationRepository;
        IMediator _mediator;

        public OrderService(IPaymentSessionFactory paymentSessionFactory, IProductStoreLocationRepository productStoreLocationRepository, IMediator mediator, IOrderValidator orderValidator, IOrderPriceCalculator orderPriceCalculator)
        {
            _paymentSessionFactory = paymentSessionFactory;
            _productStoreLocationRepository = productStoreLocationRepository;
            _mediator = mediator;
            _orderValidator = orderValidator;
            _orderPriceCalculator = orderPriceCalculator;
        }

        public async Task<PaymentProviderSession?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
        {
            if (await _mediator.Send(new GetPaymentSessionQuery(userId)) is not null)
            {
                throw new InvalidOperationException("Cannot create a payment session: there is an existing payment session!");
            }

            var items = await _mediator.Send(new GetUserCartItemsDetailedQuery(userId));

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
            var cartItems = await _mediator.Send(new GetUserCartItemsQuery(userId));
            await _productStoreLocationRepository.UpdateStock(cartItems);
            await _mediator.Send(new EraseUserCartCommand(userId));
            await _mediator.Send(new DeletePaymentSessionCommand(userId));
        }
    }
}