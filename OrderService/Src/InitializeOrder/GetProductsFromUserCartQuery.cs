using MediatR;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public record GetProductsFromUserCartQuery(Guid UserId, string JwtToken) : IRequest<Result<List<CartProductMinimalModel>>>;