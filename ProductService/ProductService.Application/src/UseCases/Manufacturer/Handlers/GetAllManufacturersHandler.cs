using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Application.Persistence;
using ProductService.Application.UseCases.Manufacturer.Queries;
using ProductService.Domain.Entities;

namespace ProductService.Application.UseCases.Manufacturer.Handlers;

public class GetAllManufacturersHandler : IRequestHandler<GetAllManufacturersQuery, List<ManufacturerEntity>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetAllManufacturersHandler> _logger;

    public GetAllManufacturersHandler(ILogger<GetAllManufacturersHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ManufacturerEntity>> Handle(GetAllManufacturersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        // TODO: pagination
        var result = await _context.Manufacturers.ToListAsync();
        _logger.LogDebug("Retrieved manufacturers: {@Manufacturers}", result);
        return result;
    }
}