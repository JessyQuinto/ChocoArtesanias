using ChocoArtesanias.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChocoArtesanias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? category = null,
        [FromQuery] string? search = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? artisan = null,
        [FromQuery] string? origin = null,
        [FromQuery] bool? featured = null,
        [FromQuery] bool? inStock = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null)
    {
        var result = await _productService.GetProductsAsync(
            page, limit, category, search, minPrice, maxPrice,
            artisan, origin, featured, inStock, sortBy, sortOrder);

        return Ok(new { Success = true, Data = result });
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedProducts()
    {
        var products = await _productService.GetFeaturedProductsAsync();
        return Ok(new { Success = true, Data = products });
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] string q,
        [FromQuery] string? category = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { Message = "El término de búsqueda es requerido" });

        var result = await _productService.SearchProductsAsync(q, category, minPrice, maxPrice);
        return Ok(new { Success = true, Data = result });
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug(string slug)
    {
        var product = await _productService.GetProductBySlugAsync(slug);
        
        if (product == null)
            return NotFound(new { Message = "Producto no encontrado" });

        return Ok(new { Success = true, Data = product });
    }
}
