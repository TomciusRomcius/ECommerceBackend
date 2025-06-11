using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Queries;

public record GetProductsFromStoreQuery(int StoreLocationId) : IRequest<List<DetailedProductModel>>;