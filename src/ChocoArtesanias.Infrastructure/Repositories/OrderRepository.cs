using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;
using ChocoArtesanias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChocoArtesanias.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(AppDbContext context, ILogger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Order?> GetByIdAsync(Guid orderId)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedido por ID: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedido por número: {OrderNumber}", orderNumber);
            throw;
        }
    }

    public async Task<Order?> GetUserOrderByIdAsync(Guid orderId, Guid userId)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedido del usuario: OrderId={OrderId}, UserId={UserId}", orderId, userId);
            throw;
        }
    }    public async Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId, int page = 1, int limit = 20, string? status = null)
    {
        try
        {
            IQueryable<Order> query = _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingAddress)
                .Where(o => o.UserId == userId);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status.ToLower() == status.ToLower());
            }

            return await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedidos del usuario: {UserId}", userId);
            throw;
        }
    }

    public async Task<int> GetUserOrdersCountAsync(Guid userId, string? status = null)
    {
        try
        {
            var query = _context.Orders.Where(o => o.UserId == userId);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status.ToLower() == status.ToLower());
            }

            return await query.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al contar pedidos del usuario: {UserId}", userId);
            throw;
        }
    }    public async Task<IEnumerable<Order>> GetAllOrdersAsync(int page = 1, int limit = 20, string? status = null)
    {
        try
        {
            IQueryable<Order> query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingAddress);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status.ToLower() == status.ToLower());
            }

            return await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los pedidos");
            throw;
        }
    }

    public async Task<int> GetAllOrdersCountAsync(string? status = null)
    {
        try
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status.ToLower() == status.ToLower());
            }

            return await query.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al contar todos los pedidos");
            throw;
        }
    }

    public async Task<Order> CreateAsync(Order order)
    {
        try
        {
            // Generar número de pedido único
            order.OrderNumber = await GenerateOrderNumberAsync();
            order.CreatedAt = DateTime.UtcNow;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Pedido creado exitosamente: {OrderNumber}", order.OrderNumber);
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear pedido");
            throw;
        }
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        try
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Pedido actualizado: {OrderNumber}", order.OrderNumber);
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar pedido: {OrderId}", order.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid orderId)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Pedido eliminado: {OrderId}", orderId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar pedido: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(Guid orderId)
    {
        try
        {
            return await _context.Orders.AnyAsync(o => o.Id == orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia del pedido: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<bool> CanCancelOrderAsync(Guid orderId, Guid userId)
    {
        try
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return false;

            // Solo se pueden cancelar pedidos en estado Pending o Processing
            return order.Status.ToLower() is "pending" or "processing";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar si se puede cancelar el pedido: {OrderId}", orderId);
            throw;
        }
    }

    public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .Take(count)
                .ToListAsync();
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
            var stats = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);

            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas de pedidos");
            throw;
        }
    }

    private async Task<string> GenerateOrderNumberAsync()
    {
        var date = DateTime.UtcNow.ToString("yyyyMMdd");
        var randomSuffix = new Random().Next(1000, 9999);
        var orderNumber = $"ORD-{date}-{randomSuffix}";

        // Verificar que sea único
        while (await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber))
        {
            randomSuffix = new Random().Next(1000, 9999);
            orderNumber = $"ORD-{date}-{randomSuffix}";
        }

        return orderNumber;
    }
}
