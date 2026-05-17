using ECommerceBackend.Utils.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;
using StoreService.Application.UseCases.Store.Queries;

namespace StoreService.Application.UseCases.Store.Handlers;

public class GetProductsFromStoreHandler
    : IRequestHandler<GetProductsFromStoreQuery, Page<ProductStoreLocationDetails>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetProductsFromStoreHandler> _logger;

    public GetProductsFromStoreHandler(ILogger<GetProductsFromStoreHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Page<ProductStoreLocationDetails>> Handle(
        GetProductsFromStoreQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered {FunctionName}", nameof(Handle));
        _logger.LogDebug(
            "Fetching products in store {StoreLocationId}, page number: {PageNumber} page size: {PageSize}",
            request.StoreLocationId,
            request.PageNumber,
            request.PageSize
        );

        Page<ProductStoreLocationDetails> page = await _context.ProductStoreLocations
            .AsNoTracking()
            .Where(psl => psl.StoreLocationId == request.StoreLocationId)
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
            .ToPageAsync(request.PageNumber, request.PageSize);

        _logger.LogDebug(
            "Retrieved products from store location id: {StoreLocationId}: {@Page}",
            request.StoreLocationId,
            page
        );

        return page;
    }
}
