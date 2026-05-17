using ECommerceBackend.Utils.Pagination;
using MediatR;

namespace StoreService.Application.UseCases.Store.Queries;

public record GetProductsFromStoreQuery(int StoreLocationId, int PageNumber, int PageSize)
    : IRequest<Page<ProductStoreLocationDetails>>;