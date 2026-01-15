using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Application.Persistence;
using ProductService.Application.UseCases.Manufacturer.Queries;
using ProductService.Domain.Entities;

namespace ProductService.Application.UseCases.Manufacturer.Handlers;

public class GetManufacturersHandler : IRequestHandler<GetManufacturersQuery, List<ManufacturerEntity>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetManufacturersHandler> _logger;

    public GetManufacturersHandler(ILogger<GetManufacturersHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ManufacturerEntity>> Handle(GetManufacturersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug(
            "Fetching manufacturers, page number: {PageNumber} page size: {PageSize}",
            request.PageNumber,
            DatabaseContext.PageSize
        );
        List<ManufacturerEntity> result = await _context.Manufacturers
            .Skip(request.PageNumber * DatabaseContext.PageSize)
            .Take(DatabaseContext.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);
        _logger.LogDebug("Retrieved manufacturers: {@Manufacturers}", result);
        return result;
    }
}