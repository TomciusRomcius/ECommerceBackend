using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Commands;

public record AddProductToStoreCommand(ProductStoreLocationEntity ProductStoreLocation) : IRequest;