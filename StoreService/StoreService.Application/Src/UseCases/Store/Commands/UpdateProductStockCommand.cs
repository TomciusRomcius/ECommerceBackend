using MediatR;
using StoreService.Domain.Entities;

namespace StoreService.Application.UseCases.Store.Commands;

public record UpdateProductStockCommand(ProductStoreLocationEntity ProductStoreLocation) : IRequest;