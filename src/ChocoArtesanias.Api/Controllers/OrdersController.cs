using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChocoArtesanias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(OrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Crear nuevo pedido desde el carrito del usuario
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.CreateOrderAsync(userId, request);

            if (order == null)
            {
                return BadRequest(new { Message = "No se pudo crear el pedido. Verifique los datos." });
            }

            return Ok(new 
            { 
                Success = true, 
                Data = order, 
                Message = "Pedido creado exitosamente" 
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear pedido");
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Obtener historial de pedidos del usuario autenticado
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUserOrders(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? status = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _orderService.GetUserOrdersAsync(userId, page, limit, status);

            return Ok(new { Success = true, Data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedidos del usuario");
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Obtener detalles de un pedido específico del usuario
    /// </summary>
    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.GetOrderByIdAsync(orderId, userId);

            if (order == null)
            {
                return NotFound(new { Message = "Pedido no encontrado" });
            }

            return Ok(new { Success = true, Data = order });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedido: {OrderId}", orderId);
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Cancelar un pedido (solo si está en estado Pending o Processing)
    /// </summary>
    [HttpPut("{orderId:guid}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid orderId, CancelOrderRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.CancelOrderAsync(orderId, userId, request);

            if (order == null)
            {
                return NotFound(new { Message = "Pedido no encontrado" });
            }

            return Ok(new 
            { 
                Success = true, 
                Data = order, 
                Message = "Pedido cancelado exitosamente" 
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cancelar pedido: {OrderId}", orderId);
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Usuario no válido");
        }
        return userId;
    }
}

/// <summary>
/// Controlador administrativo para gestión de pedidos
/// </summary>
[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminOrdersController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly ILogger<AdminOrdersController> _logger;

    public AdminOrdersController(OrderService orderService, ILogger<AdminOrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todos los pedidos del sistema (Admin)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllOrders(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? status = null)
    {
        try
        {
            var result = await _orderService.GetAllOrdersAsync(page, limit, status);
            return Ok(new { Success = true, Data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los pedidos");
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Actualizar estado de un pedido (Admin)
    /// </summary>
    [HttpPut("{orderId:guid}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, UpdateOrderStatusRequest request)
    {
        try
        {
            var order = await _orderService.UpdateOrderStatusAsync(orderId, request);

            if (order == null)
            {
                return NotFound(new { Message = "Pedido no encontrado" });
            }

            return Ok(new 
            { 
                Success = true, 
                Data = order, 
                Message = "Estado del pedido actualizado exitosamente" 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado del pedido: {OrderId}", orderId);
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Obtener pedidos recientes para el dashboard
    /// </summary>
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentOrders([FromQuery] int count = 10)
    {
        try
        {
            var orders = await _orderService.GetRecentOrdersAsync(count);
            return Ok(new { Success = true, Data = orders });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedidos recientes");
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Obtener estadísticas de pedidos
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetOrdersStatistics()
    {
        try
        {
            var stats = await _orderService.GetOrdersStatisticsAsync();
            return Ok(new { Success = true, Data = stats });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas de pedidos");
            return StatusCode(500, new { Message = "Error interno del servidor" });
        }
    }
}
