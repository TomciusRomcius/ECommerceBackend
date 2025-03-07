using ECommerce.Application.UseCases.ShippingAddress.Commands;
using ECommerce.Domain.Repositories.ShippingAddress;
using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Handlers
{
    public class UpdateShippingAddressHandler : IRequestHandler<UpdateShippingAddressCommand>
    {
        readonly IShippingAddressRepository _shippingAddressRepository;

        public UpdateShippingAddressHandler(IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepository = shippingAddressRepository;
        }

        public async Task Handle(UpdateShippingAddressCommand request, CancellationToken cancellationToken)
        {
            await _shippingAddressRepository.UpdateAddressAsync(request.UpdateAddress);
        }
    }
}