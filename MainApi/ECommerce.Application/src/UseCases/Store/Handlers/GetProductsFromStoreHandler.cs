using ECommerce.Application.src.UseCases.Store.Queries;
using ECommerce.Domain.src.Models;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Store.Handlers;

public class GetProductsFromStoreHandler : IRequestHandler<GetProductsFromStoreQuery, List<DetailedProductModel>>
{
    private readonly ILogger<GetProductsFromStoreHandler> _logger;
    private readonly DatabaseContext _context;

    public GetProductsFromStoreHandler(ILogger<GetProductsFromStoreHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<DetailedProductModel>> Handle(GetProductsFromStoreQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        IQueryable<DetailedProductModel> query = from psl in _context.ProductStoreLocations
                                                 where psl.StoreLocationId == request.StoreLocationId
                                                 join p in _context.Products on psl.ProductId equals p.ProductId
                                                 select new DetailedProductModel(
                                                     psl.ProductId,
                                                     p.Name,
                                                     p.Description,
                                                     p.Price,
                                                     p.ManufacturerId,
                                                     p.CategoryId,
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