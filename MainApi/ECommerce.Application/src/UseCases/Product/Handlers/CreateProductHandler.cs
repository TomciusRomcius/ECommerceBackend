using ECommerce.Application.src.UseCases.Product.Commands;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Product.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ProductEntity>>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductEntity>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productEntity = new ProductEntity(request.Name, request.Description, request.Price, request.ManufacturerId,
                request.CategoryId);

        return await _productRepository.CreateAsync(productEntity);
    }
}