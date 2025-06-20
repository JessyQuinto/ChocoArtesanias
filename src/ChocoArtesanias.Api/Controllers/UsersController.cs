using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChocoArtesanias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        var profile = await _userService.GetProfileAsync(userId);
        
        if (profile == null)
            return NotFound(new { Message = "Usuario no encontrado" });

        return Ok(new { Success = true, Data = profile });
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        var userId = GetCurrentUserId();
        var profile = await _userService.UpdateProfileAsync(userId, request);
        
        if (profile == null)
            return NotFound(new { Message = "Usuario no encontrado" });        return Ok(new { Success = true, Data = profile, Message = "Perfil actualizado exitosamente" });
    }

    // TODO: Address endpoints commented out since Address is now an owned entity
    // These endpoints should be removed or refactored if standalone address management is needed
    /*
    [HttpPost("addresses")]
    public async Task<IActionResult> AddAddress(AddressRequest request)
    {
        var userId = GetCurrentUserId();
        var address = await _userService.AddAddressAsync(userId, request);
        
        return Ok(new { Success = true, Data = address, Message = "Dirección agregada exitosamente" });
    }

    [HttpPut("addresses/{addressId:guid}")]
    public async Task<IActionResult> UpdateAddress(Guid addressId, AddressRequest request)
    {
        var userId = GetCurrentUserId();
        var address = await _userService.UpdateAddressAsync(userId, addressId, request);
        
        if (address == null)
            return NotFound(new { Message = "Dirección no encontrada" });

        return Ok(new { Success = true, Data = address, Message = "Dirección actualizada exitosamente" });
    }

    [HttpDelete("addresses/{addressId:guid}")]
    public async Task<IActionResult> DeleteAddress(Guid addressId)
    {
        var userId = GetCurrentUserId();
        var success = await _userService.DeleteAddressAsync(userId, addressId);
        
        if (!success)
            return NotFound(new { Message = "Dirección no encontrada" });

        return Ok(new { Success = true, Message = "Dirección eliminada exitosamente" });
    }
    */

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
