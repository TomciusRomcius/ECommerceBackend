using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Commands;

public record CreateStoreLocationCommand(CreateStoreLocationModel StoreLocation) : IRequest<StoreLocationEntity?>;