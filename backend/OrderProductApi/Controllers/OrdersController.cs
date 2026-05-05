using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderProductApi.Data;
using OrderProductApi.Models;
using OrderProductApi.Requests;

namespace OrderProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ShopContext _context;

    public OrdersController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        var ordersQuery = _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .AsQueryable();

        if (!isAdmin)
        {
            ordersQuery = ordersQuery.Where(o => o.UserId == userId);
        }

        var orders = await ordersQuery.OrderByDescending(o => o.CreatedAt).ToListAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();
        if (!User.IsInRole("Admin") && order.UserId != userId) return Forbid();

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

        if (request.Items == null || !request.Items.Any())
            return BadRequest(new { message = "Order must include at least one item." });

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

        if (products.Count != productIds.Count)
            return BadRequest(new { message = "One or more products are invalid." });

        var items = new List<OrderItem>();
        decimal total = 0;

        foreach (var itemRequest in request.Items)
        {
            if (itemRequest.Quantity <= 0)
                return BadRequest(new { message = "Quantity must be greater than zero." });

            var product = products.Single(p => p.Id == itemRequest.ProductId);
            if (product.InventoryCount < itemRequest.Quantity)
                return BadRequest(new { message = $"Insufficient inventory for {product.Name}." });

            product.InventoryCount -= itemRequest.Quantity;
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

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var createdOrder = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrder);
    }
}
