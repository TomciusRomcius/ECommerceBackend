using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.Manufacturer.Queries;

public record GetAllManufacturersQuery : IRequest<List<ManufacturerEntity>>;