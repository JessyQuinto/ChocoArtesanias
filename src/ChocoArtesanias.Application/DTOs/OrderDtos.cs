using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.DTOs;

// Request DTOs
public class CreateOrderRequest
{
    public ShippingAddressDto ShippingAddress { get; set; } = new();
    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentDetailsDto? PaymentDetails { get; set; }
    public string? Notes { get; set; }
}

public class ShippingAddressDto
{
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class PaymentDetailsDto
{
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? Cvv { get; set; }
    public string? CardHolderName { get; set; }
}

public class CancelOrderRequest
{
    public string Reason { get; set; } = string.Empty;
}

public class UpdateOrderStatusRequest
{
    public string Status { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string? TrackingNumber { get; set; }
}

// Response DTOs
public class OrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = [];
    public OrderAddressDto? ShippingAddress { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Shipping { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public List<OrderStatusHistoryDto> StatusHistory { get; set; } = [];
    public OrderTrackingDto? Tracking { get; set; }
}

public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public OrderProductDto? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
}

public class OrderProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}

public class OrderAddressDto
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class OrderStatusHistoryDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Note { get; set; }
}

public class OrderTrackingDto
{
    public string? Carrier { get; set; }
    public string? TrackingNumber { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}

public class OrderSummaryDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int ItemCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}

public class PaginatedOrdersDto
{
    public List<OrderSummaryDto> Orders { get; set; } = [];
    public PaginationDto Pagination { get; set; } = new();
}

public class PaginationDto
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
}
