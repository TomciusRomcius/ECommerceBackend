using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Queries;

public record GetShippingAddressesQuery(Guid UserId) : IRequest<List<ShippingAddressEntity>>;