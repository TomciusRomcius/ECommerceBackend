using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.UseCases.ShippingAddress.Commands;

public record UpdateShippingAddressCommand(UpdateShippingAddressModel UpdateAddress) : IRequest;