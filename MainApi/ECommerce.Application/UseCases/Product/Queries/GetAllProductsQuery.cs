using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Queries;

public record GetAllProductsQuery : IRequest<List<ProductEntity>>;