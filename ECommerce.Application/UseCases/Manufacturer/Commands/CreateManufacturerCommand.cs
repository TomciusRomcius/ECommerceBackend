using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Commands;

public record CreateManufacturerCommand(string Name) : IRequest<Result<int>>;