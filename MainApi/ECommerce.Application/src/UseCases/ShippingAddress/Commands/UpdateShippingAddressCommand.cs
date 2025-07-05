using ECommerce.Domain.src.Models;
using MediatR;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Commands;

public record UpdateShippingAddressCommand(UpdateShippingAddressModel UpdateAddress) : IRequest;