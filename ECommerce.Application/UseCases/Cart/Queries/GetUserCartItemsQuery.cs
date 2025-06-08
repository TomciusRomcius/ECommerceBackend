using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Queries;

public record GetUserCartItemsQuery(Guid UserId) : IRequest<Result<List<CartProductEntity>>>;