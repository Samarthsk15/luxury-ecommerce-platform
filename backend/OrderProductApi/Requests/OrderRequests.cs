namespace OrderProductApi.Requests;

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderRequest
{
    public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
}
