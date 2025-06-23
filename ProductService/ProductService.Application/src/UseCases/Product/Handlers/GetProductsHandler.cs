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
    private readonly ILogger<GetProductsHandler> _logger;

    public GetProductsHandler(ILogger<GetProductsHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ProductEntity>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        // TODO: pagination
        List<ProductEntity> products;
        if (request.ProductIds.Any())
        {
            products = await _context.Products
                .Where(p => request.ProductIds.Contains(p.ProductId))
                .ToListAsync();
        }
        else products = await _context.Products.ToListAsync(cancellationToken);
        
        _logger.LogDebug("Retrieved products: {@Products}", products);
        return products;
    }
}