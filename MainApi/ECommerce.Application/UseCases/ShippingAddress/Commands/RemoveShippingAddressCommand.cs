using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Commands;

public record RemoveShippingAddressCommand(Guid UserId) : IRequest;