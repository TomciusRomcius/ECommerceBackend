using ECommerce.Application.UseCases.Product.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Handlers;

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