using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Queries;

public record GetAllManufacturersQuery : IRequest<List<ManufacturerEntity>>;