using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Commands;

public record AddShippingAddressCommand(ShippingAddressEntity Address) : IRequest;