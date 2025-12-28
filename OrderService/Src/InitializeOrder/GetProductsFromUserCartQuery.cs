using MediatR;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public record GetProductsFromUserCartQuery(Guid UserId) : IRequest<Result<List<CartProductMinimalModel>>>;