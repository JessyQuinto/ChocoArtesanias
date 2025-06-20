using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;
using ChocoArtesanias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChocoArtesanias.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Producer)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Producer)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Producer)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetFeaturedAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Producer)
            .Where(p => p.Featured)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredAsync(
        int page = 1, 
        int limit = 20, 
        string? category = null, 
        string? search = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null, 
        string? artisan = null, 
        string? origin = null, 
        bool? featured = null, 
        bool? inStock = null, 
        string? sortBy = null, 
        string? sortOrder = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Producer)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category!.Slug == category);
        }

        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(p => 
                p.Name.ToLower().Contains(searchLower) ||
                p.Description.ToLower().Contains(searchLower) ||
                p.Artisan.ToLower().Contains(searchLower) ||
                p.Origin.ToLower().Contains(searchLower));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (!string.IsNullOrEmpty(artisan))
        {
            query = query.Where(p => p.Artisan.ToLower().Contains(artisan.ToLower()));
        }

        if (!string.IsNullOrEmpty(origin))
        {
            query = query.Where(p => p.Origin.ToLower().Contains(origin.ToLower()));
        }

        if (featured.HasValue)
        {
            query = query.Where(p => p.Featured == featured.Value);
        }

        if (inStock.HasValue && inStock.Value)
        {
            query = query.Where(p => p.Stock > 0);
        }

        var totalCount = await query.CountAsync();

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "price" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),
            "name" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            "date" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(p => p.CreatedAt)
                : query.OrderBy(p => p.CreatedAt),
            "rating" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(p => p.Rating)
                : query.OrderBy(p => p.Rating),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        // Apply pagination
        var products = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<IEnumerable<Product>> SearchAsync(
        string query, 
        string? category = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null)
    {
        var searchQuery = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Producer)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            var searchLower = query.ToLower();
            searchQuery = searchQuery.Where(p => 
                p.Name.ToLower().Contains(searchLower) ||
                p.Description.ToLower().Contains(searchLower) ||
                p.Artisan.ToLower().Contains(searchLower) ||
                p.Origin.ToLower().Contains(searchLower) ||
                p.Category!.Name.ToLower().Contains(searchLower) ||
                p.Producer!.Name.ToLower().Contains(searchLower));
        }

        if (!string.IsNullOrEmpty(category))
        {
            searchQuery = searchQuery.Where(p => p.Category!.Slug == category);
        }

        if (minPrice.HasValue)
        {
            searchQuery = searchQuery.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            searchQuery = searchQuery.Where(p => p.Price <= maxPrice.Value);
        }

        return await searchQuery
            .OrderByDescending(p => p.Rating)
            .ThenByDescending(p => p.Featured)
            .ThenByDescending(p => p.CreatedAt)
            .Take(50) // Limit search results
            .ToListAsync();
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        try
        {
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar producto: {ProductId}", product.Id);
            throw;
        }
    }

    public async Task<Product> CreateAsync(Product product)
    {
        try
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear producto");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar producto: {ProductId}", id);
            throw;
        }
    }
}
