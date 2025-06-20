using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChocoArtesanias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetCurrentUserId();
        var cart = await _cartService.GetCartAsync(userId);
        
        return Ok(new { Success = true, Data = cart });
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(CartItemRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cartItem = await _cartService.AddToCartAsync(userId, request);
            
            if (cartItem == null)
                return NotFound(new { Message = "Producto no encontrado" });

            return Ok(new { Success = true, Data = cartItem, Message = "Producto agregado al carrito" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("items/{itemId:guid}")]
    public async Task<IActionResult> UpdateCartItem(Guid itemId, UpdateCartItemRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cartItem = await _cartService.UpdateCartItemAsync(userId, itemId, request);
            
            if (cartItem == null)
                return NotFound(new { Message = "Artículo del carrito no encontrado" });

            return Ok(new { Success = true, Data = cartItem, Message = "Cantidad actualizada" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("items/{itemId:guid}")]
    public async Task<IActionResult> RemoveFromCart(Guid itemId)
    {
        var userId = GetCurrentUserId();
        var success = await _cartService.RemoveFromCartAsync(userId, itemId);
        
        if (!success)
            return NotFound(new { Message = "Artículo del carrito no encontrado" });

        return Ok(new { Success = true, Message = "Producto eliminado del carrito" });
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetCurrentUserId();
        await _cartService.ClearCartAsync(userId);
        
        return Ok(new { Success = true, Message = "Carrito vaciado" });
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Usuario no autenticado");
        }
        return userId;
    }
}
