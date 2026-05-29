using Microsoft.AspNetCore.Identity;
using OrderProductApi.Models;

namespace OrderProductApi.Data;

public static class SeedData
{
    public static void EnsureSeedData(ShopContext context, IPasswordHasher<User> passwordHasher)
    {
        if (!context.Users.Any())
        {
            var admin = new User
            {
                Name = "Admin",
                Email = "admin@shop.local",
                Role = "Admin"
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@123");
            context.Users.Add(admin);
        }

        if (!context.Products.Any())
        {
            var initialProducts = new List<Product>
            {
                new Product { Name = "Saffron (Kesar) - Premium Kashmiri", Category = "groceries", Description = "Pure Kashmiri saffron threads.", Price = 2500.00m, InventoryCount = 25, ImageUrl = "https://images.unsplash.com/photo-1596043144730-1c8b0b4b77a4?w=400&h=300&fit=crop" },
                new Product { Name = "Basmati Rice - Premium Long Grain", Category = "groceries", Description = "Aromatic long-grain basmati rice.", Price = 180.00m, InventoryCount = 100, ImageUrl = "https://images.unsplash.com/photo-1586201375761-83865001e31c?w=400&h=300&fit=crop" },
                new Product { Name = "Handwoven Banarasi Silk Saree", Category = "fashion", Description = "Exquisite handwoven silk saree.", Price = 8500.00m, InventoryCount = 15, ImageUrl = "https://images.unsplash.com/photo-1610030469983-98e550d6193c?w=400&h=300&fit=crop" },
                new Product { Name = "Organic Turmeric Powder", Category = "groceries", Description = "Pure organic turmeric powder.", Price = 95.00m, InventoryCount = 50, ImageUrl = "https://images.unsplash.com/photo-1596043144730-1c8b0b4b77a4?w=400&h=300&fit=crop" },
                new Product { Name = "Madras Coffee Beans", Category = "groceries", Description = "Premium Arabica coffee beans.", Price = 320.00m, InventoryCount = 30, ImageUrl = "https://images.unsplash.com/photo-1559056199-641a0ac8b55e?w=400&h=300&fit=crop" },
                new Product { Name = "Ghee - Pure Desi Cow Ghee", Category = "groceries", Description = "Traditional clarified butter.", Price = 450.00m, InventoryCount = 40, ImageUrl = "https://images.unsplash.com/photo-1586511925558-a4c6376fe65f?w=400&h=300&fit=crop" },
                new Product { Name = "Kolhapuri Chappals - Handcrafted Leather", Category = "fashion", Description = "Authentic Kolhapuri leather sandals.", Price = 1200.00m, InventoryCount = 20, ImageUrl = "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=400&h=300&fit=crop" },
                new Product { Name = "Darjeeling Tea - First Flush", Category = "groceries", Description = "Premium first flush Darjeeling tea.", Price = 280.00m, InventoryCount = 35, ImageUrl = "https://images.unsplash.com/photo-1544787219-7f47ccb76574?w=400&h=300&fit=crop" },
                new Product { Name = "Handmade Brass Pooja Thali Set", Category = "home", Description = "Traditional brass pooja thali.", Price = 650.00m, InventoryCount = 25, ImageUrl = "https://images.unsplash.com/photo-1609592806500-4b6c3e9c6b8a?w=400&h=300&fit=crop" },
                new Product { Name = "Kerala Coconut Oil - Virgin", Category = "beauty", Description = "Pure virgin coconut oil.", Price = 220.00m, InventoryCount = 45, ImageUrl = "https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=400&h=300&fit=crop" }
            };
            
            context.Products.AddRange(initialProducts);

            // Generate at least 100 meaningful products per category
            var random = new Random();
            var categories = new[] { "electronics", "fashion", "home", "beauty", "toys", "sports", "books", "groceries", "gaming", "automotive" };
            var prefixes = new[] { "Premium", "Ultra", "Classic", "Modern", "Smart", "Eco-friendly", "Luxury", "Essential", "Pro", "Vintage" };
            var baseNames = new[] { "Gadget", "Device", "Apparel", "Tool", "Accessory", "Equipment", "Kit", "Set", "Bundle", "Edition" };

            var generated = new List<Product>();
            foreach (var cat in categories)
            {
                for (int j = 1; j <= 100; j++)
                {
                    var pre = prefixes[random.Next(prefixes.Length)];
                    var baseN = baseNames[random.Next(baseNames.Length)];
                    generated.Add(new Product
                    {
                        Name = $"{pre} {capFirst(cat)} {baseN} #{j}",
                        Category = cat,
                        Description = $"{pre} {baseN} for {cat} — crafted for quality and performance.",
                        Price = random.Next(10, 1500) + 0.99m,
                        InventoryCount = random.Next(0, 500),
                        ImageUrl = $"https://via.placeholder.com/400x300?text={cat}+{j}"
                    });
                }
            }

            context.Products.AddRange(generated);
            context.SaveChanges();
        }
    }

    private static string capFirst(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + (s.Length > 1 ? s.Substring(1) : string.Empty);
    }
}
