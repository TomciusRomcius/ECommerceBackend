using ECommerce.Application.src.UseCases.ShippingAddress.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Handlers;

public class GetShippingAddressesHandler : IRequestHandler<GetShippingAddressesQuery, List<ShippingAddressEntity>>
{
    private readonly IShippingAddressRepository _shippingAddressRepository;

    public GetShippingAddressesHandler(IShippingAddressRepository shippingAddressRepository)
    {
        _shippingAddressRepository = shippingAddressRepository;
    }

    public async Task<List<ShippingAddressEntity>> Handle(GetShippingAddressesQuery request,
        CancellationToken cancellationToken)
    {
        return await _shippingAddressRepository.GetAddresses(request.UserId.ToString());
    }
}