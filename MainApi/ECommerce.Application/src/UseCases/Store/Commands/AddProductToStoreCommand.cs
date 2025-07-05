using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Commands;

public record AddProductToStoreCommand(ProductStoreLocationEntity ProductStoreLocation) : IRequest<ResultError?>;