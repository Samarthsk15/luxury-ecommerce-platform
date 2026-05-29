namespace OrderProductApi.Services;
using OrderProductApi.Models;
using OrderProductApi.Requests;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersAsync(bool isAdmin, int userId);
    Task<Order?> GetOrderByIdAsync(int id, bool isAdmin, int userId);
    Task<Order> CreateOrderAsync(CreateOrderRequest request, int userId);
}
