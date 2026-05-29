using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderProductApi.Requests;
using OrderProductApi.Services;

namespace OrderProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var isAdmin = User.IsInRole("Admin");
            var orders = await _orderService.GetOrdersAsync(isAdmin, userId);
            
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving orders.", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        try
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var isAdmin = User.IsInRole("Admin");
            var order = await _orderService.GetOrderByIdAsync(id, isAdmin, userId);

            if (order == null) return NotFound();
            return Ok(order);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the order.", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var order = await _orderService.CreateOrderAsync(request, userId);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the order.", details = ex.Message });
        }
    }
}
