using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChocoArtesanias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        if (!response.Success)
        {
            return BadRequest(new { response.Message });
        }
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        if (!response.Success)
        {
            return Unauthorized(new { response.Message });
        }
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request);
        if (!response.Success)
        {
            return BadRequest(new { response.Message });
        }
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request)
    {
        var success = await _authService.LogoutAsync(request);
        if (!success)
        {
            return BadRequest(new { Message = "Error al cerrar sesión" });
        }
        return Ok(new { Success = true, Message = "Sesión cerrada exitosamente" });
    }
}
