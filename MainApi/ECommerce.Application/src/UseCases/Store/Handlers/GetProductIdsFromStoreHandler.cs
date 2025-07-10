using ECommerce.Application.src.UseCases.Store.Queries;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Store.Handlers;

public class GetProductIdsFromStoreHandler : IRequestHandler<GetProductIdsFromStoreQuery, List<int>>
{
    private readonly ILogger<GetProductIdsFromStoreHandler> _logger;
    private readonly DatabaseContext _context;

    public GetProductIdsFromStoreHandler(ILogger<GetProductIdsFromStoreHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<int>> Handle(GetProductIdsFromStoreQuery request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug("Getting product ids from store: {StoreLocationId}", request.StoreLocationId);

        List<int> result = await _context.ProductStoreLocations
            .Where(psl => psl.StoreLocationId == request.StoreLocationId)
            .Select(psl => psl.ProductId)
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.LogDebug("Retrieved product ids: {@ProductIds}", result);
        return result;
    }
}