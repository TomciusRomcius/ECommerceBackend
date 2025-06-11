using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Queries;

public record GetAllLocationsQuery : IRequest<List<StoreLocationEntity>>;