using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Interfaces;

public interface IProducerRepository
{
    Task<Producer?> GetByIdAsync(int id);
    Task<IEnumerable<Producer>> GetAllAsync();
    Task<Producer?> GetByIdWithProductsAsync(int id);
}
