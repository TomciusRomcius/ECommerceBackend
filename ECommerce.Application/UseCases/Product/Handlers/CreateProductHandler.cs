using ECommerce.Application.UseCases.Product.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductEntity>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductEntity> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // TODO: return result to handle errors
        var productEntity = new ProductEntity(request.Name, request.Description, request.Price, request.ManufacturerId,
                request.CategoryId);

        Result<ProductEntity> result = await _productRepository.CreateAsync(productEntity);
        return result.GetValue();
    }
}