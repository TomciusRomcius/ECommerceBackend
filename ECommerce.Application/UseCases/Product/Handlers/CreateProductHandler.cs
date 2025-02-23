using ECommerce.Application.UseCases.Product.Commands;
using ECommerce.Domain.Entities.Product;
using ECommerce.Domain.Repositories.Product;
using MediatR;

namespace ECommerce.Application.UseCases.Product.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductEntity>
    {
        readonly IProductRepository _productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductEntity> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.CreateAsync(
                new ProductEntity(request.Name, request.Description, request.Price, request.ManufacturerId, request.CategoryId)
            );
        }
    }
}