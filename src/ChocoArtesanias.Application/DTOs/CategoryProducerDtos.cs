namespace ChocoArtesanias.Application.DTOs;

// Category DTOs
public record CategoryResponse(
    int Id,
    string Name,
    string Slug,
    string Description,
    string Image,
    int ProductCount);

public record CategoryDetailResponse(
    int Id,
    string Name,
    string Slug,
    string Description,
    string Image,
    IEnumerable<ProductResponse> Products,
    int ProductCount);

// Producer DTOs
public record ProducerResponse(
    int Id,
    string Name,
    string Description,
    string Location,
    string Image,
    bool Featured,
    int FoundationYear,
    int ProductCount);

public record ProducerDetailResponse(
    int Id,
    string Name,
    string Description,
    string Location,
    string Image,
    bool Featured,
    int FoundationYear,
    IEnumerable<ProductResponse> Products,
    ContactInfo? Contact);

public record ContactInfo(
    string? Phone,
    string? Email,
    string? Address);
