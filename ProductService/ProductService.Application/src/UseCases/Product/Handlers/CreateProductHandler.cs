using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Persistence;
using ProductService.Application.UseCases.Product.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Utils;

namespace ProductService.Application.UseCases.Product.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ProductEntity>>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<CreateProductHandler> _logger;

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

        await _context.Products.AddAsync(productEntity, cancellationToken);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            foreach (string imageKey in request.ImageKeys)
            {
                if (string.IsNullOrWhiteSpace(imageKey))
                {
                    continue;
                }

                await _context.ProductImages.AddAsync(
                    new ProductImageEntity(productEntity.ProductId, imageKey.Trim()),
                    cancellationToken);
            }

            if (request.ImageKeys.Count > 0)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
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