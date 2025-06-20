namespace ChocoArtesanias.Application.DTOs;

// Cart DTOs
public record CartItemRequest(int ProductId, int Quantity);

public record UpdateCartItemRequest(int Quantity);

public record CartItemResponse(
    string Id,
    int ProductId,
    ProductCartInfo Product,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);

public record ProductCartInfo(
    int Id,
    string Name,
    string Image,
    decimal Price,
    int Stock);

public record CartResponse(
    string UserId,
    IEnumerable<CartItemResponse> Items,
    int ItemCount,
    decimal Subtotal,
    decimal Tax,
    decimal Total,
    DateTime UpdatedAt);

// Common Response DTOs
public record ApiResponse<T>(
    bool Success,
    T? Data,
    string Message);

public record ApiResponse(
    bool Success,
    string Message);
