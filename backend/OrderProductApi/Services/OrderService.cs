namespace OrderProductApi.Services;
using OrderProductApi.Models;
using OrderProductApi.Repositories;
using OrderProductApi.Requests;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(bool isAdmin, int userId)
    {
        return await _orderRepository.GetAllOrdersAsync(isAdmin ? null : userId);
    }

    public async Task<Order?> GetOrderByIdAsync(int id, bool isAdmin, int userId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null) return null;
        if (!isAdmin && order.UserId != userId) throw new UnauthorizedAccessException("You are not authorized to view this order.");
        return order;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request, int userId)
    {
        if (request.Items == null || !request.Items.Any())
            throw new ArgumentException("Order must include at least one item.");

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = (await _productRepository.GetByIdsAsync(productIds)).ToList();

        if (products.Count != productIds.Count)
            throw new ArgumentException("One or more products are invalid.");

        var items = new List<OrderItem>();
        decimal total = 0;

        foreach (var itemRequest in request.Items)
        {
            if (itemRequest.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var product = products.Single(p => p.Id == itemRequest.ProductId);
            if (product.InventoryCount < itemRequest.Quantity)
                throw new ArgumentException($"Insufficient inventory for {product.Name}.");

            product.InventoryCount -= itemRequest.Quantity;
            await _productRepository.UpdateAsync(product); // Update inventory

            var lineTotal = product.Price * itemRequest.Quantity;
            total += lineTotal;

            items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemRequest.Quantity,
                UnitPrice = product.Price
            });
        }

        var order = new Order
        {
            UserId = userId,
            Status = "Pending",
            TotalAmount = total,
            Items = items
        };

        var createdOrder = await _orderRepository.CreateOrderAsync(order);
        return await _orderRepository.GetOrderByIdAsync(createdOrder.Id) ?? createdOrder;
    }
}
