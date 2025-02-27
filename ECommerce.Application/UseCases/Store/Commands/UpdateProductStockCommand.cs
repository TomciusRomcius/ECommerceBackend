using ECommerce.Domain.Entities.ProductStoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Commands
{
    public record UpdateProductStockCommand(ProductStoreLocationEntity ProductStoreLocation) : IRequest;

}