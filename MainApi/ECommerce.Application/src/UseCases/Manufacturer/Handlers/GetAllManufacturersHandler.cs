using ECommerce.Application.src.UseCases.Manufacturer.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Manufacturer.Handlers;

public class GetAllManufacturersHandler : IRequestHandler<GetAllManufacturersQuery, List<ManufacturerEntity>>
{
    private readonly ILogger<GetAllManufacturersHandler> _logger;
    private readonly DatabaseContext _context;

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
        List<ManufacturerEntity> result = await _context.Manufacturers.ToListAsync();
        _logger.LogDebug("Retrieved manufacturers: {@Manufacturers}", result);
        return result;
    }
}