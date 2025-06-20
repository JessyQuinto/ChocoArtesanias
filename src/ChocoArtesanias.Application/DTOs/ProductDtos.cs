namespace ChocoArtesanias.Application.DTOs;

// Product DTOs
public record ProductResponse(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    decimal Price,
    decimal? DiscountedPrice,
    string Image,
    List<string> Images,
    Guid CategoryId,
    string CategoryName,
    Guid ProducerId,
    string ProducerName,
    int Stock,
    bool Featured,
    double Rating,
    string Artisan,
    string Origin,
    DateTime CreatedAt);

public record ProductDetailResponse(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    decimal Price,
    decimal? DiscountedPrice,
    string Image,
    List<string> Images,
    CategoryResponse Category,
    ProducerResponse Producer,
    int Stock,
    bool Featured,
    double Rating,
    string Artisan,
    string Origin,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record ProductSearchResponse(
    IEnumerable<ProductResponse> Products,
    int TotalResults,
    IEnumerable<string> Suggestions);

public record PaginationResponse(
    int CurrentPage,
    int TotalPages,
    int TotalItems,
    int ItemsPerPage);

public record ProductListResponse(
    IEnumerable<ProductResponse> Products,
    PaginationResponse Pagination);
