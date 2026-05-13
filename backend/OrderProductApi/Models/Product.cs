using System.ComponentModel.DataAnnotations;

namespace OrderProductApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1000000.0)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int InventoryCount { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;
}
