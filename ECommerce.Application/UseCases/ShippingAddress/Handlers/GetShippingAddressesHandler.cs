using ECommerce.Application.UseCases.ShippingAddress.Queries;
using ECommerce.Domain.Entities.ShippingAddress;
using ECommerce.Domain.Repositories.ShippingAddress;
using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Handlers
{
    public class GetShippingAddressesHandler : IRequestHandler<GetShippingAddressesQuery, List<ShippingAddressEntity>>
    {
        readonly IShippingAddressRepository _shippingAddressRepository;

        public GetShippingAddressesHandler(IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepository = shippingAddressRepository;
        }

        public async Task<List<ShippingAddressEntity>> Handle(GetShippingAddressesQuery request, CancellationToken cancellationToken)
        {
            return await _shippingAddressRepository.GetAddresses(request.UserId.ToString());
        }
    }
}