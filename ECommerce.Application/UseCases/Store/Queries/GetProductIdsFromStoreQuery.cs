using MediatR;

namespace ECommerce.Application.UseCases.Store.Queries;

public record GetProductIdsFromStoreQuery(int StoreLocationId) : IRequest<List<int>>;