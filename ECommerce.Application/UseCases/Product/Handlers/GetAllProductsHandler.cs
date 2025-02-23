using ECommerce.Application.UseCases.Product.Queries;
using ECommerce.Domain.Entities.Product;
using ECommerce.Domain.Repositories.Product;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductEntity>>
    {
        readonly IProductRepository _productRepository;

        public GetAllProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductEntity>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAll();
        }
    }
}