using ECommerce.Application.UseCases.Product.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Handlers;

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