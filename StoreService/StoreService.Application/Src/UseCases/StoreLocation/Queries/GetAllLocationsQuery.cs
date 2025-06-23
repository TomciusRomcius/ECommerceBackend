using MediatR;
using StoreService.Domain.Entities;

namespace StoreService.Application.UseCases.StoreLocation.Queries;

public record GetAllLocationsQuery : IRequest<List<StoreLocationEntity>>;