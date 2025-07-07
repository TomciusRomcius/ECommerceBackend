using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Manufacturer.Commands;

public record CreateManufacturerCommand(string Name) : IRequest<Result<int>>;