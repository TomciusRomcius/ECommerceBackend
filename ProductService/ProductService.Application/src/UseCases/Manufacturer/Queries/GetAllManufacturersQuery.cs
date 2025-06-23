using MediatR;
using ProductService.Domain.Entities;

namespace ProductService.Application.UseCases.Manufacturer.Queries;

public record GetAllManufacturersQuery : IRequest<List<ManufacturerEntity>>;