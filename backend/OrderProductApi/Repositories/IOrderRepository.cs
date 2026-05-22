namespace OrderProductApi.Repositories;
using OrderProductApi.Models;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrdersAsync(int? userId = null);
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
}
