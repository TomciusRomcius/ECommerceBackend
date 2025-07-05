using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.StoreLocation.Queries;

public record GetAllLocationsQuery : IRequest<List<StoreLocationEntity>>;