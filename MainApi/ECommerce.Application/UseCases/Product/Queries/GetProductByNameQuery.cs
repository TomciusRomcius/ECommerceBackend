using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Queries;

public record GetProductByNameQuery(string Name) : IRequest<ProductEntity?>;