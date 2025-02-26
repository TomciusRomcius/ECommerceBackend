using ECommerce.Domain.Entities.ShippingAddress;
using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Commands
{
    public record AddShippingAddressCommand(ShippingAddressEntity Address) : IRequest;
}