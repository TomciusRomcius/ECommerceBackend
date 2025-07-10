using ECommerce.Application.src.UseCases.StoreLocation.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.StoreLocation.Handlers;

public class GetAllLocationsHandler : IRequestHandler<GetAllLocationsQuery, List<StoreLocationEntity>>
{
    private readonly ILogger<GetAllLocationsHandler> _logger;
    private readonly DatabaseContext _context;

    public GetAllLocationsHandler(ILogger<GetAllLocationsHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<StoreLocationEntity>> Handle(GetAllLocationsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        List<StoreLocationEntity> result = await _context.StoreLocations.ToListAsync(cancellationToken);
        _logger.LogDebug("Get all results: {@GetAllLocationsResult}", result);
        return result;
    }
}