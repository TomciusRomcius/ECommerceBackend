using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Application.Persistence;
using ProductService.Application.UseCases.Product.Queries;
using ProductService.Domain.Entities;

namespace ProductService.Application.UseCases.Product.Handlers;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductEntity>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetProductsByIdHandler> _logger;

    public GetProductsHandler(ILogger<GetProductsByIdHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ProductEntity>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered {FunctionName}", nameof(GetProductsHandler));
        _logger.LogDebug(
            "Fetching products, page number: {PageNumber} page size: {PageSize}",
            request.PageNumber,
            DatabaseContext.PageSize
        );
        List<ProductEntity> products = await _context.Products
            .Skip(request.PageNumber * DatabaseContext.PageSize)
            .Take(DatabaseContext.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);
        
        _logger.LogDebug("Retrieved products: {@Products}", products);
        return products;
    }
}