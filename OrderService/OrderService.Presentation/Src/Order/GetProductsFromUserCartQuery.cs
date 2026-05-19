using MediatR;
using OrderService.Utils;

namespace OrderService.Presentation.Order;

public record GetProductsFromUserCartQuery(Guid UserId) : IRequest<Result<List<CartProductMinimalModel>>>;