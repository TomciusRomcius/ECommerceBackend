using ECommerce.Domain.src.Models;
using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Queries;

public record GetProductsFromStoreQuery(int StoreLocationId) : IRequest<List<DetailedProductModel>>;