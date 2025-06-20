using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponse> GetCartAsync(Guid userId)
    {
        var cartItems = await _cartRepository.GetCartItemsAsync(userId);
        
        var items = cartItems.Select(MapToCartItemResponse);
        var subtotal = cartItems.Sum(c => c.TotalPrice);
        var tax = subtotal * 0.19m; // 19% IVA in Colombia
        var total = subtotal + tax;

        return new CartResponse(
            userId.ToString(),
            items,
            cartItems.Count(),
            subtotal,
            tax,
            total,
            cartItems.Any() ? cartItems.Max(c => c.UpdatedAt) : DateTime.UtcNow
        );
    }

    public async Task<CartItemResponse?> AddToCartAsync(Guid userId, CartItemRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null) return null;

        if (product.Stock < request.Quantity)
            throw new InvalidOperationException("No hay suficiente stock disponible");

        // Check if item already exists in cart
        var existingItem = await _cartRepository.GetCartItemAsync(userId, request.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
            existingItem.UpdatedAt = DateTime.UtcNow;
            
            if (product.Stock < existingItem.Quantity)
                throw new InvalidOperationException("No hay suficiente stock disponible");

            await _cartRepository.UpdateCartItemAsync(existingItem);
            return MapToCartItemResponse(existingItem);
        }

        // Create new cart item
        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = product.DiscountedPrice ?? product.Price,
            Product = product
        };

        await _cartRepository.AddCartItemAsync(cartItem);
        return MapToCartItemResponse(cartItem);
    }

    public async Task<CartItemResponse?> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemRequest request)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId, userId);
        if (cartItem == null) return null;

        if (cartItem.Product!.Stock < request.Quantity)
            throw new InvalidOperationException("No hay suficiente stock disponible");

        cartItem.Quantity = request.Quantity;
        cartItem.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateCartItemAsync(cartItem);
        return MapToCartItemResponse(cartItem);
    }

    public async Task<bool> RemoveFromCartAsync(Guid userId, Guid cartItemId)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId, userId);
        if (cartItem == null) return false;

        await _cartRepository.RemoveCartItemAsync(cartItemId, userId);
        return true;
    }

    public async Task<bool> ClearCartAsync(Guid userId)
    {
        await _cartRepository.ClearCartAsync(userId);
        return true;
    }

    private static CartItemResponse MapToCartItemResponse(CartItem cartItem)
    {
        return new CartItemResponse(
            cartItem.Id.ToString(),
            cartItem.ProductId,
            new ProductCartInfo(
                cartItem.Product!.Id,
                cartItem.Product.Name,
                cartItem.Product.ImageUrl,
                cartItem.Product.Price,
                cartItem.Product.Stock
            ),
            cartItem.Quantity,
            cartItem.UnitPrice,
            cartItem.TotalPrice
        );
    }
}
