using MediatR;
using StoreService.Domain.Entities;

namespace StoreService.Application.UseCases.Store.Queries;

public record GetProductsFromStoreQuery(int StoreLocationId, int PageNumber) : IRequest<List<ProductStoreLocationEntity>>;