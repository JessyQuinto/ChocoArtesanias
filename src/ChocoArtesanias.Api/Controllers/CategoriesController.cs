using ChocoArtesanias.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChocoArtesanias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(new { Success = true, Data = categories });
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetCategoryBySlug(string slug)
    {
        var category = await _categoryService.GetCategoryBySlugAsync(slug);
        
        if (category == null)
            return NotFound(new { Message = "Categoría no encontrada" });

        return Ok(new { Success = true, Data = category });
    }
}
