using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Repositories.ProductStoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Order.NotificationHandlers
{
    public class ChargeSucceededNotificationHandler : INotificationHandler<ChargeSucceededNotification>
    {
        readonly IProductStoreLocationRepository _productStoreLocationRepository;
        readonly IMediator _mediator;

        public ChargeSucceededNotificationHandler(IProductStoreLocationRepository productStoreLocationRepository, IMediator mediator)
        {
            _productStoreLocationRepository = productStoreLocationRepository;
            _mediator = mediator;
        }

        public async Task Handle(ChargeSucceededNotification notification, CancellationToken cancellationToken)
        {
            var cartItems = await _mediator.Send(new GetUserCartItemsQuery(notification.UserId));
            await _productStoreLocationRepository.UpdateStock(cartItems);
            await _mediator.Send(new EraseUserCartCommand(notification.UserId));
            await _mediator.Send(new DeletePaymentSessionCommand(notification.UserId));
        }
    }
}