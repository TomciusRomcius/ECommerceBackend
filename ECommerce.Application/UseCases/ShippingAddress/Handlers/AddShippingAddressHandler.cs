using ECommerce.Application.UseCases.ShippingAddress.Commands;
using ECommerce.Domain.Repositories.ShippingAddress;
using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Handlers
{
    public class AddShippingAddressHandler : IRequestHandler<AddShippingAddressCommand>
    {
        readonly IShippingAddressRepository _shippingAddressRepository;

        public AddShippingAddressHandler(IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepository = shippingAddressRepository;
        }

        public async Task Handle(AddShippingAddressCommand request, CancellationToken cancellationToken)
        {
            await _shippingAddressRepository.AddAddressAsync(request.Address);
        }
    }
}