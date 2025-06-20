using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category?> GetBySlugAsync(string slug);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetBySlugWithProductsAsync(string slug);
}
