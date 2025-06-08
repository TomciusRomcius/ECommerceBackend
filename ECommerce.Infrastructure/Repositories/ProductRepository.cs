using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace ECommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IPostgresService _postgresService;
    private readonly IValidator<ProductEntity> _productValidator;
    private readonly IValidator<UpdateProductModel> _updateProductValidator;

    public ProductRepository(IPostgresService postgresService, 
        IValidator<ProductEntity> productValidator, 
        IValidator<UpdateProductModel> updateProductValidator)
    {
        _postgresService = postgresService;
        _productValidator = productValidator;
        _updateProductValidator = updateProductValidator;
    }

    public async Task<Result<ProductEntity>> CreateAsync(ProductEntity product)
    {
        List<ValidationFailure> errors = _productValidator.Validate(product).Errors;
        if (errors.Any())
        {
            return new Result<ProductEntity>(
                ResultUtils.ValidationFailuresToResultErrors(errors)
            );
        }

        var query = @"
                    INSERT INTO products(name, description, price, manufacturerId, categoryId) 
                    VALUES ($1, $2, $3, $4, $5)
                    RETURNING productId;
                ";

        QueryParameter[] parameters =
        [
            new(product.Name),
            new(product.Description),
            new(product.Price),
            new(product.ManufacturerId),
            new(product.CategoryId)
        ];

        ProductEntity? result = null;

        object? id = await _postgresService.ExecuteScalarAsync(query, parameters.ToArray());

        if (id is int)
        {
            result = product;
            result.ProductId = Convert.ToInt32(id);
        }

        return new Result<ProductEntity>(result);
    }

    public async Task<ResultError?> UpdateAsync(UpdateProductModel product)
    {
        List<ValidationFailure> errors = _updateProductValidator.Validate(product).Errors;
        if (errors.Any())
        {
            return ResultUtils.ValidationFailuresToResultError(errors);
        }

        var query = @"
                    UPDATE products
                    SET name = COALESCE($1, name)
                    SET description = COALESCE($2, description)
                    SET price = COALESCE($3, price)
                    SET manufacturerId = COALESCE($4, manufacturerId)
                    SET categoryId = COALESCE($5, categoryId)
                    WHERE productId = $6;
                ";

        QueryParameter[] parameters =
        [
            new(product.Name),
            new(product.Description),
            new(product.Price),
            new(product.ManufacturerId),
            new(product.CategoryId),
            new(product.ProductId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters.ToArray());
        return null;
    }

    public async Task DeleteAsync(int productId)
    {
        var query = @"
                    DELETE FROM products WHERE productId = $1; 
                ";

        QueryParameter[] parameters =
        [
            new(productId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters.ToArray());
    }

    public Task<ProductEntity?> FindByIdAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductEntity?> FindByNameAsync(string productName)
    {
        var query = @"
                SELECT * FROM products;
            ";

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
        ProductEntity? result = null;

        Dictionary<string, object>? row = rows[0];

        if (row is not null)
            result = new ProductEntity(
                row.GetColumn<int>("productid"),
                row.GetColumn<string>("name"),
                row.GetColumn<string>("description"),
                row.GetColumn<decimal>("price"), // TODO: decimal
                row.GetColumn<int>("manufacturerid"),
                row.GetColumn<int>("categoryid")
            );

        return result;
    }

    public async Task<List<ProductEntity>> GetAll()
    {
        var query = @"
                SELECT * FROM products;
            ";

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
        List<ProductEntity> result = new List<ProductEntity>();

        foreach (Dictionary<string, object> row in rows)
            result.Add(new ProductEntity(
                row.GetColumn<int>("productid"),
                row.GetColumn<string>("name"),
                row.GetColumn<string>("description"),
                row.GetColumn<decimal>("price"), // TODO: decimal
                row.GetColumn<int>("manufacturerid"),
                row.GetColumn<int>("categoryid")
            ));

        return result;
    }

    public Task<List<ProductEntity>> GetAllInCategory(int categoryId)
    {
        throw new NotImplementedException();
    }
}