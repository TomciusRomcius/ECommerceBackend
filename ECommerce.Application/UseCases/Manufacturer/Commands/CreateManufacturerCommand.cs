using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Commands
{
    public record CreateManufacturerCommand(string Name) : IRequest<Result<ManufacturerEntity>>;
}