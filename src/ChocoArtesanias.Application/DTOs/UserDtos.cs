namespace ChocoArtesanias.Application.DTOs;

// User DTOs
public record UpdateProfileRequest(string FirstName, string LastName, string? Phone);

public record AddressRequest(
    string Name, 
    string FullName, 
    string Address, 
    string City, 
    string PostalCode, 
    string Phone, 
    bool IsDefault = false);

public record AddressResponse(
    string Id,
    string Name,
    string FullName,
    string Address,
    string City,
    string PostalCode,
    string Phone,
    bool IsDefault);

public record UserProfileResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    IEnumerable<AddressResponse> Addresses,
    DateTime CreatedAt,
    DateTime UpdatedAt);
