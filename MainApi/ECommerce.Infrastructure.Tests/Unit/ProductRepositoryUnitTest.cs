using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Npgsql;

namespace ECommerce.Infrastructure.Tests.Unit;

public class ProductRepositoryUnitTest
{
    private readonly ProductRepository _productRepository;
    private readonly Mock<IPostgresService> _postgresServiceMock = new Mock<IPostgresService>();
    private readonly Mock<IValidator<ProductEntity>> _productValidatorMock = new Mock<IValidator<ProductEntity>>();
    
    public ProductRepositoryUnitTest()
    {
        _productRepository = new ProductRepository(
            _postgresServiceMock.Object,
            _productValidatorMock.Object,
            new Mock<IValidator<UpdateProductModel>>().Object
        );
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnResultError_WhenDatabaseExceptionOccurs()
    {
        var postgresException = new PostgresException("",
            "",
            "",
            PostgresErrorCodes.UniqueViolation
        );

        _productValidatorMock
            .Setup(x => x.Validate(It.IsAny<ProductEntity>()))
            .Returns(new ValidationResult());
        
        _postgresServiceMock.Setup(
            ps => ps.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>()
        )).ThrowsAsync(postgresException);

        var productEntity = new ProductEntity("Product name", 
            "Product description", 
            5.99m,
            -1, 
            -1
        );
        Result<ProductEntity> result = await _productRepository.CreateAsync(productEntity);
        
        Assert.NotEmpty(result.Errors);
        Assert.Equal(ResultErrorType.VALIDATION_ERROR, result.Errors.First().ErrorType);
    }
}