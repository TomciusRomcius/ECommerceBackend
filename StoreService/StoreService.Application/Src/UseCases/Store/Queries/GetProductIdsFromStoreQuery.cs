using MediatR;

namespace StoreService.Application.UseCases.Store.Queries;

public record GetProductIdsFromStoreQuery(int StoreLocationId) : IRequest<List<int>>;