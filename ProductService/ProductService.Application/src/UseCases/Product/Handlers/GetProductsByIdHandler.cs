using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Application.Persistence;
using ProductService.Application.UseCases.Product.Queries;
using ProductService.Domain.Entities;

namespace ProductService.Application.UseCases.Product.Handlers;

public class GetProductsByIdHandler : IRequestHandler<GetProductsByIdQuery, List<ProductEntity>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GetProductsByIdHandler> _logger;

    public GetProductsByIdHandler(ILogger<GetProductsByIdHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ProductEntity>> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        List<ProductEntity> products;
        if (request.ProductIds.Any())
        {
            products = await _context.Products
                .Where(p => request.ProductIds.Contains(p.ProductId))
                .ToListAsync(cancellationToken: cancellationToken);
        }
        else products = await _context.Products.ToListAsync(cancellationToken);
        
        _logger.LogDebug("Retrieved products: {@Products}", products);
        return products;
    }
}