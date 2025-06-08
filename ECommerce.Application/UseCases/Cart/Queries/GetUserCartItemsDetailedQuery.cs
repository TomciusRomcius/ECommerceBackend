using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Queries;

public record GetUserCartItemsDetailedQuery(Guid UserId) : IRequest<Result<List<CartProductModel>>>;