using MediatR;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Commands;

public record RemoveShippingAddressCommand(Guid UserId) : IRequest;