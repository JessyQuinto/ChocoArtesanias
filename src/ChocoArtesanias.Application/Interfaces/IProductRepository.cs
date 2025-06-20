using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product?> GetBySlugAsync(string slug);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetFeaturedAsync();
    Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredAsync(
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
        string? sortBy = null,        string? sortOrder = null);
    Task<IEnumerable<Product>> SearchAsync(string query, string? category = null, decimal? minPrice = null, decimal? maxPrice = null);
    Task<Product> UpdateAsync(Product product);
    Task<Product> CreateAsync(Product product);
    Task DeleteAsync(Guid id);
}
