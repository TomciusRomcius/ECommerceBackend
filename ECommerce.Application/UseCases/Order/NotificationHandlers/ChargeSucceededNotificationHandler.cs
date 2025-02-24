using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Repositories.ProductStoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Order.NotificationHandlers
{
    public class ChargeSucceededNotificationHandler : INotificationHandler<ChargeSucceededNotification>
    {
        readonly ICartService _cartService;
        readonly IProductStoreLocationRepository _productStoreLocationRepository;
        readonly IMediator _mediator;

        public ChargeSucceededNotificationHandler(ICartService cartService, IProductStoreLocationRepository productStoreLocationRepository, IMediator mediator)
        {
            _cartService = cartService;
            _productStoreLocationRepository = productStoreLocationRepository;
            _mediator = mediator;
        }

        public async Task Handle(ChargeSucceededNotification notification, CancellationToken cancellationToken)
        {
            // TODO: separate 
            var cartItems = await _cartService.GetAllUserItems(notification.UserId.ToString());
            await _productStoreLocationRepository.UpdateStock(cartItems);
            await _cartService.WipeAsync(notification.UserId);
            await _mediator.Send(new DeletePaymentSessionCommand(notification.UserId));
        }
    }
}