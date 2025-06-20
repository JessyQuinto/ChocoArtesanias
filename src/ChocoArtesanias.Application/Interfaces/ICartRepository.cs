using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Interfaces;

public interface ICartRepository
{
    Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId);
    Task<CartItem?> GetCartItemAsync(Guid userId, int productId);
    Task AddCartItemAsync(CartItem cartItem);
    Task UpdateCartItemAsync(CartItem cartItem);
    Task RemoveCartItemAsync(Guid cartItemId, Guid userId);
    Task ClearCartAsync(Guid userId);
    Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId, Guid userId);
}
