using ECommerce.Domain.Entities.Product;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Commands
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int ManufacturerId,
        int CategoryId) : IRequest<ProductEntity>;
}