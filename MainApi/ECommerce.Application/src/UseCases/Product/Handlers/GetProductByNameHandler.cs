using ECommerce.Application.src.UseCases.Product.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Product.Handlers;

public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, ProductEntity?>
{
    private readonly ILogger<GetProductByNameHandler> _logger;
    private readonly DatabaseContext _context;

    public GetProductByNameHandler(ILogger<GetProductByNameHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ProductEntity?> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug("Searching for product: {Name}", request.Name);
        ProductEntity? product = await _context.Products.Where(p => p.Name == request.Name).FirstOrDefaultAsync();
        _logger.LogDebug("Retrieved product: {@Product}", product);
        return product;
    }
}