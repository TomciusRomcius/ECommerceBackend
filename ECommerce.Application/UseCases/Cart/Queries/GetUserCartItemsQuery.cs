using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Queries;

public record GetUserCartItemsQuery(Guid UserId) : IRequest<List<CartProductEntity>>;