using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Application.Persistence;
using ProductService.Domain.Entities;
using ProductService.Domain.Utils;

namespace ProductService.Application.Services;

public interface ICategoriesService
{
    public Task<List<CategoryEntity>> GetAllCategories();
    public Task<Result<int>> CreateCategory(CategoryEntity entity);
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

    public async Task<List<CategoryEntity>> GetAllCategories()
    {
        _logger.LogTrace("Entered GetAllCategories");
        // TODO: pagination
        var result = await _context.Categories.ToListAsync();
        _logger.LogDebug("Retrieved categories: {}", result);
        return result;
    }

    public async Task<Result<int>> CreateCategory(CategoryEntity entity)
    {
        _logger.LogTrace("Entered CreateCategory");
        _logger.LogDebug("Creating category: {}", entity.Name);

        await _context.Categories.AddAsync(entity);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Succesfully created category: {}", entity);
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