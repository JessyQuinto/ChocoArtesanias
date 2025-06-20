using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Interfaces;

namespace ChocoArtesanias.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductListResponse> GetProductsAsync(
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
        var (products, totalCount) = await _productRepository.GetFilteredAsync(
            page, limit, category, search, minPrice, maxPrice, 
            artisan, origin, featured, inStock, sortBy, sortOrder);

        var productResponses = products.Select(MapToProductResponse);
        
        var pagination = new PaginationResponse(
            page,
            (int)Math.Ceiling((double)totalCount / limit),
            totalCount,
            limit
        );

        return new ProductListResponse(productResponses, pagination);
    }

    public async Task<ProductDetailResponse?> GetProductBySlugAsync(string slug)
    {
        var product = await _productRepository.GetBySlugAsync(slug);
        if (product == null) return null;

        return new ProductDetailResponse(
            product.Id,
            product.Name,
            product.Slug,
            product.Description,
            product.Price,
            product.DiscountedPrice,
            product.ImageUrl,
            product.Images,
            new CategoryResponse(
                product.Category!.Id,
                product.Category.Name,
                product.Category.Slug,
                product.Category.Description,
                product.Category.ImageUrl,
                0 // Product count not needed here
            ),
            new ProducerResponse(
                product.Producer!.Id,
                product.Producer.Name,
                product.Producer.Description,
                product.Producer.Location,
                product.Producer.ImageUrl,
                product.Producer.Featured,
                product.Producer.FoundationYear,
                0 // Product count not needed here
            ),
            product.Stock,
            product.Featured,
            product.Rating,
            product.Artisan,
            product.Origin,
            product.CreatedAt,
            product.UpdatedAt
        );
    }

    public async Task<IEnumerable<ProductResponse>> GetFeaturedProductsAsync()
    {
        var products = await _productRepository.GetFeaturedAsync();
        return products.Select(MapToProductResponse);
    }

    public async Task<ProductSearchResponse> SearchProductsAsync(
        string query, 
        string? category = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null)
    {
        var products = await _productRepository.SearchAsync(query, category, minPrice, maxPrice);
        var productResponses = products.Select(MapToProductResponse);

        // Simple suggestions based on product names (could be enhanced)
        var suggestions = products
            .Select(p => p.Name)
            .Where(name => name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(5)
            .ToList();

        return new ProductSearchResponse(
            productResponses,
            productResponses.Count(),
            suggestions
        );
    }

    private static ProductResponse MapToProductResponse(Domain.Entities.Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Slug,
            product.Description,
            product.Price,
            product.DiscountedPrice,
            product.ImageUrl,
            product.Images,
            product.CategoryId,
            product.Category?.Name ?? "",
            product.ProducerId,
            product.Producer?.Name ?? "",
            product.Stock,
            product.Featured,
            product.Rating,
            product.Artisan,
            product.Origin,
            product.CreatedAt
        );
    }
}
