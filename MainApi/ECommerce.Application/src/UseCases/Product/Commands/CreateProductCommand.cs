using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Product.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int ManufacturerId,
    int CategoryId) : IRequest<Result<ProductEntity>>;