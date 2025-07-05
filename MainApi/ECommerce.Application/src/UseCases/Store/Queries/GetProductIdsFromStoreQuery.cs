using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Queries;

public record GetProductIdsFromStoreQuery(int StoreLocationId) : IRequest<List<int>>;