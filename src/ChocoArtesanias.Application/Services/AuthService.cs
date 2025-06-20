using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace ChocoArtesanias.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(IUserRepository userRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) != null)
        {
            return new AuthResponse(false, string.Empty, string.Empty, null!, "El correo electrónico ya está en uso.");
        }

        using var hmac = new HMACSHA512();
        var passwordSalt = hmac.Key;
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "Customer"
        };

        await _userRepository.AddAsync(user);

        var token = _tokenService.CreateToken(user);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);
        var userResponse = new UserResponse(user.Id.ToString(), user.FirstName, user.LastName, user.Email, user.Role);

        return new AuthResponse(true, token, refreshToken, userResponse, "Usuario registrado exitosamente.");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponse(false, string.Empty, string.Empty, null!, "Credenciales inválidas.");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return new AuthResponse(false, string.Empty, string.Empty, null!, "Credenciales inválidas.");
            }
        }

        var token = _tokenService.CreateToken(user);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);
        var userResponse = new UserResponse(user.Id.ToString(), user.FirstName, user.LastName, user.Email, user.Role);

        return new AuthResponse(true, token, refreshToken, userResponse, "Inicio de sesión exitoso.");
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (refreshToken == null || !refreshToken.IsActive)
        {
            return new RefreshTokenResponse(false, string.Empty, string.Empty, "Token de actualización inválido.");
        }

        // Revoke old refresh token
        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(refreshToken);

        // Create new tokens
        var newToken = _tokenService.CreateToken(refreshToken.User!);
        var newRefreshToken = await CreateRefreshTokenAsync(refreshToken.UserId);

        return new RefreshTokenResponse(true, newToken, newRefreshToken, "Token actualizado exitosamente.");
    }

    public async Task<bool> LogoutAsync(LogoutRequest request)
    {
        try
        {
            var userId = _tokenService.GetUserIdFromToken(request.Token);
            if (userId.HasValue)
            {
                await _refreshTokenRepository.RevokeAllUserTokensAsync(userId.Value);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<string> CreateRefreshTokenAsync(Guid userId)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = _tokenService.CreateRefreshToken(),
            UserId = userId,
            ExpiryDate = DateTime.UtcNow.AddDays(30), // 30 days expiry
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(refreshToken);
        return refreshToken.Token;
    }
}
