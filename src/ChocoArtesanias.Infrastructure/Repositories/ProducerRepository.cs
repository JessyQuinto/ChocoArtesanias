using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;
using ChocoArtesanias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChocoArtesanias.Infrastructure.Repositories;

public class ProducerRepository : IProducerRepository
{
    private readonly AppDbContext _context;

    public ProducerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Producer?> GetByIdAsync(int id)
    {
        return await _context.Producers.FindAsync(id);
    }

    public async Task<IEnumerable<Producer>> GetAllAsync()
    {
        return await _context.Producers
            .OrderByDescending(p => p.Featured)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Producer?> GetByIdWithProductsAsync(int id)
    {
        return await _context.Producers
            .Include(p => p.Products)
                .ThenInclude(pr => pr.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
