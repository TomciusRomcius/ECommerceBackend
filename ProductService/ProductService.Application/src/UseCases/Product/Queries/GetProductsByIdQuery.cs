using MediatR;
using ProductService.Domain.Entities;

namespace ProductService.Application.UseCases.Product.Queries;

public record GetProductsByIdQuery(List<int> ProductIds) : IRequest<List<ProductEntity>>;