using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Interfaces;

namespace ChocoArtesanias.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        
        return categories.Select(c => new CategoryResponse(
            c.Id,
            c.Name,
            c.Slug,
            c.Description,
            c.ImageUrl,
            c.Products.Count
        ));
    }

    public async Task<CategoryDetailResponse?> GetCategoryBySlugAsync(string slug)
    {
        var category = await _categoryRepository.GetBySlugWithProductsAsync(slug);
        if (category == null) return null;

        var products = category.Products.Select(p => new ProductResponse(
            p.Id,
            p.Name,
            p.Slug,
            p.Description,
            p.Price,
            p.DiscountedPrice,
            p.ImageUrl,
            p.Images,
            p.CategoryId,
            category.Name,
            p.ProducerId,
            p.Producer?.Name ?? "",
            p.Stock,
            p.Featured,
            p.Rating,
            p.Artisan,
            p.Origin,
            p.CreatedAt
        ));

        return new CategoryDetailResponse(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.ImageUrl,
            products,
            category.Products.Count
        );
    }
}
