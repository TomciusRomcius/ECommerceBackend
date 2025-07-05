using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using MediatR;

namespace ECommerce.Application.src.UseCases.StoreLocation.Commands;

public record CreateStoreLocationCommand(CreateStoreLocationModel StoreLocation) : IRequest<StoreLocationEntity?>;