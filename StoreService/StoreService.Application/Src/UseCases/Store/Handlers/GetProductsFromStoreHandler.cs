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
        _logger.LogTrace("Entered Handle");
        IQueryable<ProductStoreLocationEntity> query = from psl in _context.ProductStoreLocations
            where psl.StoreLocationId == request.StoreLocationId
            select new ProductStoreLocationEntity(
                psl.StoreLocationId,
                psl.ProductId,
                psl.Stock
            );

        var result = await query.ToListAsync();

        _logger.LogDebug(
            "Retrieved products from store location id: {StoreLocationId}: {@Products}",
            request.StoreLocationId,
            result
        );

        return result;
    }
}