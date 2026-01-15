using MediatR;
using ProductService.Domain.Entities;

namespace ProductService.Application.UseCases.Product.Queries;

public record GetProductsQuery(int PageNumber) : IRequest<List<ProductEntity>>;