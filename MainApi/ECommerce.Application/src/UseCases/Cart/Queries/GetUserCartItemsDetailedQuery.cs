using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Queries;

public record GetUserCartItemsDetailedQuery(Guid UserId) : IRequest<Result<List<CartProductModel>>>;