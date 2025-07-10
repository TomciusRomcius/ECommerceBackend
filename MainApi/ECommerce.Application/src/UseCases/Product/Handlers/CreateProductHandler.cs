using ECommerce.Application.src.UseCases.Product.Commands;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Product.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ProductEntity>>
{
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly DatabaseContext _context;

    public CreateProductHandler(ILogger<CreateProductHandler> logger, DatabaseContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<ProductEntity>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        var productEntity = new ProductEntity(request.Name, request.Description, request.Price, request.ManufacturerId,
                request.CategoryId);

        _logger.LogDebug("Creating product: {@Product}", productEntity);

        await _context.Products.AddAsync(productEntity);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown while saving changes");
            return new Result<ProductEntity>([
                new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create the product")
            ]);
        }

        _logger.LogInformation(
            "Created product. Name: {Name}, ProductId: {ProductId}",
            productEntity.Name,
            productEntity.ProductId
        );
        return new Result<ProductEntity>(productEntity);
    }
}