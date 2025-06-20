using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid orderId);
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<Order?> GetUserOrderByIdAsync(Guid orderId, Guid userId);
    Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId, int page = 1, int limit = 20, string? status = null);
    Task<int> GetUserOrdersCountAsync(Guid userId, string? status = null);
    Task<IEnumerable<Order>> GetAllOrdersAsync(int page = 1, int limit = 20, string? status = null);
    Task<int> GetAllOrdersCountAsync(string? status = null);
    Task<Order> CreateAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(Guid orderId);
    Task<bool> ExistsAsync(Guid orderId);
    Task<bool> CanCancelOrderAsync(Guid orderId, Guid userId);
    Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10);
    Task<Dictionary<string, int>> GetOrdersStatisticsAsync();
}
