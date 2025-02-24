using ECommerce.Domain.Entities.Manufacturer;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Commands
{
    public record CreateManufacturerCommand(string Name) : IRequest<ManufacturerEntity>;
}