using ECommerce.Domain.Entities.ShippingAddress;
using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Queries
{
    public record GetShippingAddressesQuery(Guid UserId) : IRequest<List<ShippingAddressEntity>>;
}
