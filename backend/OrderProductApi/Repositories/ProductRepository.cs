namespace OrderProductApi.Repositories;
using OrderProductApi.Models;
using OrderProductApi.Data;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ShopContext _context;
    
    public ProductRepository(ShopContext context) 
    { 
        _context = context; 
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int page, int limit, string? category)
    {
        var query = _context.Products.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(category) && category.ToLower() != "all")
        {
            query = query.Where(p => p.Category.ToLower() == category.ToLower());
        }
        
        var total = await query.CountAsync();
        var products = await query.OrderBy(p => p.Id).Skip((page - 1) * limit).Take(limit).ToListAsync();
        
        return (products, total);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
