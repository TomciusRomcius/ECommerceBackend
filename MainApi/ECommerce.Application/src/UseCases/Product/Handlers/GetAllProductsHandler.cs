using ECommerce.Application.src.UseCases.Product.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Product.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductEntity>>
{
    private readonly ILogger<GetAllProductsHandler> _logger;
    private readonly DatabaseContext _context;

    public GetAllProductsHandler(ILogger<GetAllProductsHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ProductEntity>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        // TODO: pagination
        List<ProductEntity> products = await _context.Products.ToListAsync(cancellationToken: cancellationToken);
        _logger.LogDebug("Retrieved products: {@Products}", products);
        return products;
    }
}