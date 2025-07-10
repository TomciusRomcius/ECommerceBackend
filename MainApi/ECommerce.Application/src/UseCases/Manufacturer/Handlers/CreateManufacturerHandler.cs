using ECommerce.Application.src.UseCases.Manufacturer.Commands;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Manufacturer.Handlers;

public class CreateManufacturerHandler : IRequestHandler<CreateManufacturerCommand, Result<int>>
{
    private readonly ILogger<CreateManufacturerHandler> _logger;
    private readonly DatabaseContext _context;

    public CreateManufacturerHandler(
        ILogger<CreateManufacturerHandler> logger,
        DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        var entity = new ManufacturerEntity(request.Name);
        _logger.LogDebug("Creating manufacturer: {@Manufacturer}", entity);

        await _context.Manufacturers.AddAsync(entity);

        try
        {
            await _context.SaveChangesAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown while persisting to the database.");

            return new Result<int>([
                new ResultError(
                    ResultErrorType.UNKNOWN_ERROR,
                    "Failed to create manufacturer"
                )
            ]);
        }

        _logger.LogDebug("Created manufacturer with id: {Id}", entity.ManufacturerId);
        return new Result<int>(entity.ManufacturerId);
    }
}