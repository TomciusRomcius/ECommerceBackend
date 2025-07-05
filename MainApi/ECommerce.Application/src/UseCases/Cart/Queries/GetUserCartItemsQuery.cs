using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Queries;

public record GetUserCartItemsQuery(Guid UserId) : IRequest<Result<List<CartProductEntity>>>;