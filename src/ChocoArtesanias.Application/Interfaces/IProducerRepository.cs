using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Interfaces;

public interface IProducerRepository
{
    Task<Producer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Producer>> GetAllAsync();
    Task<Producer?> GetByIdWithProductsAsync(Guid id);
}
