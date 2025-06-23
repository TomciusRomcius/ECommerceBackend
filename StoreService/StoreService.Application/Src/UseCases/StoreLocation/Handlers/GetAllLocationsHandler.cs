using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;
using StoreService.Application.UseCases.StoreLocation.Queries;
using StoreService.Domain.Entities;

namespace StoreService.Application.UseCases.StoreLocation.Handlers;

public class GetAllLocationsHandler : IRequestHandler<GetAllLocationsQuery, List<StoreLocationEntity>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetAllLocationsHandler> _logger;

    public GetAllLocationsHandler(ILogger<GetAllLocationsHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<StoreLocationEntity>> Handle(GetAllLocationsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        var result = await _context.StoreLocations.ToListAsync(cancellationToken);
        _logger.LogDebug("Get all results: {@GetAllLocationsResult}", result);
        return result;
    }
}