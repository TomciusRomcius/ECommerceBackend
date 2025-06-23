using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Utils;

namespace ProductService.Application.UseCases.Product.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int ManufacturerId,
    int CategoryId) : IRequest<Result<ProductEntity>>;