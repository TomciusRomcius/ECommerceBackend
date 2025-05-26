using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Queries;

public record GetUserCartItemsDetailedQuery(Guid UserId) : IRequest<List<CartProductModel>>;