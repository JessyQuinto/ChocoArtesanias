using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChocoArtesanias.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IUserRepository userRepository,
        IProductRepository productRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<OrderDto?> CreateOrderAsync(Guid userId, CreateOrderRequest request)
    {
        try
        {
            // Validar que el usuario exista
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Usuario no encontrado: {UserId}", userId);
                return null;
            }

            // Obtener el carrito del usuario
            var cartItems = await _cartRepository.GetCartItemsAsync(userId);
            if (!cartItems.Any())
            {
                throw new InvalidOperationException("El carrito está vacío");
            }            // Validar dirección de envío - usando los datos del request directamente
            // ya que Address es ahora una owned entity
            var shippingAddress = new Address
            {
                Id = Guid.NewGuid(),
                Name = request.ShippingAddress.Name,
                FullName = request.ShippingAddress.FullName,
                StreetAddress = request.ShippingAddress.StreetAddress,
                City = request.ShippingAddress.City,
                PostalCode = request.ShippingAddress.PostalCode,
                Phone = request.ShippingAddress.Phone,
                IsDefault = false
            };

            // Validar stock de productos
            foreach (var cartItem in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product == null || product.Stock < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Stock insuficiente para el producto: {product?.Name}");
                }
            }

            // Crear el pedido
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = "Pending",
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = "Pending",                ShippingAddress = new Address
                {
                    Id = Guid.NewGuid(),
                    Name = shippingAddress.Name,
                    FullName = shippingAddress.FullName,
                    StreetAddress = shippingAddress.StreetAddress,
                    City = shippingAddress.City,
                    PostalCode = shippingAddress.PostalCode,
                    Phone = shippingAddress.Phone,
                    IsDefault = false
                },
                Items = cartItems.Select(ci => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList(),
                EstimatedDelivery = DateTime.UtcNow.AddDays(5) // 5 días de entrega estimada
            };

            // Calcular totales
            CalculateOrderTotals(order);

            // Actualizar stock de productos
            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock -= item.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
            }

            // Guardar el pedido
            var createdOrder = await _orderRepository.CreateAsync(order);

            // Limpiar el carrito
            await _cartRepository.ClearCartAsync(userId);

            _logger.LogInformation("Pedido creado exitosamente: {OrderNumber} para usuario {UserId}", 
                createdOrder.OrderNumber, userId);

            return MapToOrderDto(createdOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear pedido para usuario: {UserId}", userId);
            throw;
        }
    }

    public async Task<PaginatedOrdersDto> GetUserOrdersAsync(Guid userId, int page = 1, int limit = 20, string? status = null)
    {
        try
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId, page, limit, status);
            var totalCount = await _orderRepository.GetUserOrdersCountAsync(userId, status);

            var orderSummaries = orders.Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Status = o.Status,
                Total = o.Total,
                ItemCount = o.Items.Count,
                CreatedAt = o.CreatedAt,
                EstimatedDelivery = o.EstimatedDelivery
            }).ToList();

            return new PaginatedOrdersDto
            {
                Orders = orderSummaries,
                Pagination = new PaginationDto
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)limit),
                    TotalItems = totalCount,
                    ItemsPerPage = limit
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedidos del usuario: {UserId}", userId);
            throw;
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId)
    {
        try
        {
            var order = await _orderRepository.GetUserOrderByIdAsync(orderId, userId);
            if (order == null)
            {
                return null;
            }

            return MapToOrderDto(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedido: {OrderId} para usuario: {UserId}", orderId, userId);
            throw;
        }
    }

    public async Task<OrderDto?> CancelOrderAsync(Guid orderId, Guid userId, CancelOrderRequest request)
    {
        try
        {
            if (!await _orderRepository.CanCancelOrderAsync(orderId, userId))
            {
                throw new InvalidOperationException("El pedido no se puede cancelar en su estado actual");
            }

            var order = await _orderRepository.GetUserOrderByIdAsync(orderId, userId);
            if (order == null)
            {
                return null;
            }

            // Actualizar estado del pedido
            order.Status = "Canceled";

            // Restaurar stock de productos
            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
            }

            var updatedOrder = await _orderRepository.UpdateAsync(order);            _logger.LogInformation("Pedido cancelado: {OrderNumber} por usuario {UserId}. Razón: {Reason}", 
                order.OrderNumber, userId, request.Reason);

            return MapToOrderDto(updatedOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cancelar pedido: {OrderId} para usuario: {UserId}", orderId, userId);
            throw;
        }
    }

    // Métodos para administradores
    public async Task<PaginatedOrdersDto> GetAllOrdersAsync(int page = 1, int limit = 20, string? status = null)
    {
        try
        {
            var orders = await _orderRepository.GetAllOrdersAsync(page, limit, status);
            var totalCount = await _orderRepository.GetAllOrdersCountAsync(status);

            var orderSummaries = orders.Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Status = o.Status,
                Total = o.Total,
                ItemCount = o.Items.Count,
                CreatedAt = o.CreatedAt,
                EstimatedDelivery = o.EstimatedDelivery
            }).ToList();

            return new PaginatedOrdersDto
            {
                Orders = orderSummaries,
                Pagination = new PaginationDto
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)limit),
                    TotalItems = totalCount,
                    ItemsPerPage = limit
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los pedidos");
            throw;
        }
    }

    public async Task<OrderDto?> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequest request)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            order.Status = request.Status;

            var updatedOrder = await _orderRepository.UpdateAsync(order);            _logger.LogInformation("Estado del pedido actualizado: {OrderNumber} a {Status}. Nota: {Note}", 
                order.OrderNumber, request.Status, request.Note);

            return MapToOrderDto(updatedOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado del pedido: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<IEnumerable<OrderSummaryDto>> GetRecentOrdersAsync(int count = 10)
    {
        try
        {
            var orders = await _orderRepository.GetRecentOrdersAsync(count);
            return orders.Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Status = o.Status,
                Total = o.Total,
                ItemCount = o.Items.Count,
                CreatedAt = o.CreatedAt,
                EstimatedDelivery = o.EstimatedDelivery
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedidos recientes");
            throw;
        }
    }

    public async Task<Dictionary<string, int>> GetOrdersStatisticsAsync()
    {
        try
        {
            return await _orderRepository.GetOrdersStatisticsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas de pedidos");
            throw;
        }
    }

    private void CalculateOrderTotals(Order order)
    {
        order.Subtotal = order.Items.Sum(item => item.UnitPrice * item.Quantity);
        order.Tax = order.Subtotal * 0.19m; // 19% IVA en Colombia
        order.Shipping = order.Subtotal > 100000 ? 0 : 15000; // Envío gratis para compras > $100,000
        order.Total = order.Subtotal + order.Tax + order.Shipping;
    }

    private OrderDto MapToOrderDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            Status = order.Status,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            Subtotal = order.Subtotal,
            Tax = order.Tax,
            Shipping = order.Shipping,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            EstimatedDelivery = order.EstimatedDelivery,            ShippingAddress = order.ShippingAddress != null ? new OrderAddressDto
            {
                FullName = order.ShippingAddress.FullName,
                Address = order.ShippingAddress.StreetAddress,
                City = order.ShippingAddress.City,
                PostalCode = order.ShippingAddress.PostalCode,
                Phone = order.ShippingAddress.Phone
            } : null,
            Items = order.Items.Select(item => new OrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,                Product = item.Product != null ? new OrderProductDto
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    Slug = item.Product.Slug,
                    Image = item.Product.ImageUrl
                } : null
            }).ToList(),
            StatusHistory = new List<OrderStatusHistoryDto>
            {
                new()
                {
                    Status = order.Status,
                    Timestamp = order.CreatedAt,
                    Note = "Pedido creado"
                }
            },
            Tracking = new OrderTrackingDto
            {
                EstimatedDelivery = order.EstimatedDelivery
            }
        };
    }
}
