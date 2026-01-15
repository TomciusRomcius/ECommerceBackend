using MediatR;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

/// <returns>A sorted list that is sorted on ProductId and value being price</returns>
/// <param name="ProductIds"></param>
public record GetProductDescriptionQuery(List<int> ProductIds) : IRequest<Result<List<ProductPriceModel>>>;
