using ECommerce.Application.src.UseCases.Product.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.Product.Handlers;

public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, ProductEntity?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByNameHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductEntity?> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.FindByNameAsync(request.Name);
    }
}