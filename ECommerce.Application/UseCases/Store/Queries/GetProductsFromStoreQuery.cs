using ECommerce.Domain.Models.ProductStoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Store
{
    public record GetProductsFromStoreQuery(int StoreLocationId) : IRequest<List<DetailedProductModel>>;
}