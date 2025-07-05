using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.Product.Queries;

public record GetProductByNameQuery(string Name) : IRequest<ProductEntity?>;