using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderProductApi.Data;
using OrderProductApi.Models;

namespace OrderProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromotionsController : ControllerBase
{
    private readonly ShopContext _context;

    public PromotionsController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetActivePromotions()
    {
        var now = DateTime.UtcNow;
        var promotions = await _context.Promotions
            .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now)
            .OrderByDescending(p => p.DiscountPercentage)
            .ToListAsync();

        return Ok(promotions);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Promotion promotion)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetActivePromotions), new { id = promotion.Id }, promotion);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Promotion payload)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var promotion = await _context.Promotions.FindAsync(id);
        if (promotion == null) return NotFound();

        promotion.Title = payload.Title;
        promotion.Description = payload.Description;
        promotion.DiscountPercentage = payload.DiscountPercentage;
        promotion.StartDate = payload.StartDate;
        promotion.EndDate = payload.EndDate;
        promotion.IsActive = payload.IsActive;
        promotion.ImageUrl = payload.ImageUrl;

        await _context.SaveChangesAsync();
        return Ok(promotion);
    }
}
