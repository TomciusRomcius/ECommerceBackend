using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;
using StoreService.Application.UseCases.Store.Queries;

namespace StoreService.Application.UseCases.Store.Handlers;

public class GetProductStoreLocationsByProductIdsHandler
    : IRequestHandler<GetProductStoreLocationsByProductIdsQuery, List<ProductStoreLocationDetails>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetProductStoreLocationsByProductIdsHandler> _logger;

    public GetProductStoreLocationsByProductIdsHandler(
        ILogger<GetProductStoreLocationsByProductIdsHandler> logger,
        DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ProductStoreLocationDetails>> Handle(
        GetProductStoreLocationsByProductIdsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");

        if (request.ProductIds.Count == 0)
        {
            return [];
        }

        List<ProductStoreLocationDetails> result = await _context.ProductStoreLocations
            .AsNoTracking()
            .Where(psl => request.ProductIds.Contains(psl.ProductId))
            .Join(
                _context.StoreLocations.AsNoTracking(),
                psl => psl.StoreLocationId,
                store => store.StoreLocationId,
                (psl, store) => new ProductStoreLocationDetails
                {
                    ProductId = psl.ProductId,
                    StoreLocationId = psl.StoreLocationId,
                    Stock = psl.Stock,
                    DisplayName = store.DisplayName,
                    Address = store.Address,
                })
            .ToListAsync(cancellationToken);

        _logger.LogDebug("Retrieved product store locations: {@Result}", result);
        return result;
    }
}
