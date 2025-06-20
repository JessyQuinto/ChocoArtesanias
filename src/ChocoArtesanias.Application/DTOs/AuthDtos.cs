namespace ChocoArtesanias.Application.DTOs;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record RefreshTokenRequest(string RefreshToken);
public record LogoutRequest(string Token);

public record UserResponse(string Id, string FirstName, string LastName, string Email, string Role);

public record AuthResponse(bool Success, string Token, string RefreshToken, UserResponse User, string Message);

public record RefreshTokenResponse(bool Success, string Token, string RefreshToken, string Message);
