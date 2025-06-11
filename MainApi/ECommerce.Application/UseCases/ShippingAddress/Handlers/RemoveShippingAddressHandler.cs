using ECommerce.Application.UseCases.ShippingAddress.Commands;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Handlers;

public class RemoveShippingAddressHandler : IRequestHandler<RemoveShippingAddressCommand>
{
    private readonly IShippingAddressRepository _shippingAddressRepository;

    public RemoveShippingAddressHandler(IShippingAddressRepository shippingAddressRepository)
    {
        _shippingAddressRepository = shippingAddressRepository;
    }

    public async Task Handle(RemoveShippingAddressCommand request, CancellationToken cancellationToken)
    {
        await _shippingAddressRepository.DeleteAddressAsync(request.UserId.ToString());
    }
}