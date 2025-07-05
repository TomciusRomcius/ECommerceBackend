using ECommerce.Application.src.UseCases.ShippingAddress.Commands;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Handlers;

public class AddShippingAddressHandler : IRequestHandler<AddShippingAddressCommand>
{
    private readonly IShippingAddressRepository _shippingAddressRepository;

    public AddShippingAddressHandler(IShippingAddressRepository shippingAddressRepository)
    {
        _shippingAddressRepository = shippingAddressRepository;
    }

    public async Task Handle(AddShippingAddressCommand request, CancellationToken cancellationToken)
    {
        await _shippingAddressRepository.AddAddressAsync(request.Address);
    }
}