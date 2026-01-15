using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Application.Persistence;
using ProductService.Domain.Entities;
using ProductService.Domain.Utils;

namespace ProductService.Application.Services;

public interface ICategoriesService
{
    public Task<List<CategoryEntity>> GetCategoriesAsync(int pageNumber);
    public Task<Result<int>> CreateCategoryAsync(CategoryEntity entity);
}

public class CategoriesService : ICategoriesService
{
    private readonly DatabaseContext _context;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(ILogger<CategoriesService> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<CategoryEntity>> GetCategoriesAsync(int pageNumber)
    {
        _logger.LogTrace("Entered {FunctionName}", nameof(GetCategoriesAsync));
        _logger.LogDebug(
            "Fetching categories, page number: {PageNumber} page size: {PageSize}",
            pageNumber,
            DatabaseContext.PageSize
        );
        List<CategoryEntity> result = await _context.Categories
            .Skip(pageNumber * DatabaseContext.PageSize)
            .Take(DatabaseContext.PageSize)
            .ToListAsync();
        return result;
    }

    public async Task<Result<int>> CreateCategoryAsync(CategoryEntity entity)
    {
        _logger.LogTrace("Entered CreateCategory");
        _logger.LogDebug("Creating category: {}", entity.Name);

        await _context.Categories.AddAsync(entity);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully created category: {@Category}", entity);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception was thrown while saving changes: {}", ex);
            return new Result<int>([
                new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create the product")
            ]);
        }

        return new Result<int>(entity.CategoryId);
    }
}