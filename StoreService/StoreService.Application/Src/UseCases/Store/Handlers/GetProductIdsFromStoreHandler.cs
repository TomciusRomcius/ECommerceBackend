using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;
using StoreService.Application.UseCases.Store.Queries;

namespace StoreService.Application.UseCases.Store.Handlers;

public class GetProductIdsFromStoreHandler : IRequestHandler<GetProductIdsFromStoreQuery, List<int>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetProductIdsFromStoreHandler> _logger;

    public GetProductIdsFromStoreHandler(ILogger<GetProductIdsFromStoreHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<int>> Handle(GetProductIdsFromStoreQuery request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug("Getting product ids from store: {StoreLocationId}", request.StoreLocationId);

        var result = await _context.ProductStoreLocations
            .Where(psl => psl.StoreLocationId == request.StoreLocationId)
            .Select(psl => psl.ProductId)
            .ToListAsync(cancellationToken);

        _logger.LogDebug("Retrieved product ids: {@ProductIds}", result);
        return result;
    }
}