namespace OrderProductApi.Services;
using OrderProductApi.Models;

public interface IProductService
{
    Task<(IEnumerable<Product> Products, int TotalCount)> GetAllProductsAsync(int page, int limit, string? category);
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product payload);
    Task<Product?> UpdateProductAsync(int id, Product payload);
    Task<bool> DeleteProductAsync(int id);
}
