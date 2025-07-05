using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.Product.Queries;

public record GetAllProductsQuery : IRequest<List<ProductEntity>>;