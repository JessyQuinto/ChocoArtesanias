using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;
using ChocoArtesanias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChocoArtesanias.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
                .ThenInclude(p => p!.Category)
            .Include(c => c.Product)
                .ThenInclude(p => p!.Producer)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }    public async Task<CartItem?> GetCartItemAsync(Guid userId, Guid productId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        cartItem.UpdatedAt = DateTime.UtcNow;
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveCartItemAsync(Guid cartItemId, Guid userId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);
            
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(Guid userId)
    {
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .ToListAsync();
            
        if (cartItems.Any())
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId, Guid userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);
    }
}
