using ECommerce.Application.src.UseCases.Product.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.Product.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductEntity>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductEntity>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAll();
    }
}