using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Commands;

public record UpdateProductStockCommand(ProductStoreLocationEntity ProductStoreLocation) : IRequest;