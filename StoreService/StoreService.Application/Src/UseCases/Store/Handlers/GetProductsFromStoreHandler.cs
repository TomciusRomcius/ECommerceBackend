using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;
using StoreService.Application.UseCases.Store.Queries;
using StoreService.Domain.Entities;

namespace StoreService.Application.UseCases.Store.Handlers;

public class GetProductsFromStoreHandler : IRequestHandler<GetProductsFromStoreQuery, List<ProductStoreLocationEntity>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetProductsFromStoreHandler> _logger;

    public GetProductsFromStoreHandler(ILogger<GetProductsFromStoreHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ProductStoreLocationEntity>> Handle(GetProductsFromStoreQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered {FunctionName}", nameof(Handle));
        _logger.LogDebug(
            "Fetching products in store {StoreLocationId}, page number: {PageNumber} page size: {PageSize}",
            request.StoreLocationId,
            request.PageNumber,
            DatabaseContext.PageSize
        );
        List<ProductStoreLocationEntity> result = await _context.ProductStoreLocations
            .Skip(request.PageNumber * DatabaseContext.PageSize)
            .Take(DatabaseContext.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.LogDebug(
            "Retrieved products from store location id: {StoreLocationId}: {@Products}",
            request.StoreLocationId,
            result
        );

        return result;
    }
}