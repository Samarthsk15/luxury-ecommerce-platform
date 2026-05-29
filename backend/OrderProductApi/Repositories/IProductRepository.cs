namespace OrderProductApi.Repositories;
using OrderProductApi.Models;

public interface IProductRepository
{
    Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int page, int limit, string? category);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
    Task<Product> AddAsync(Product product);
    Task<Product?> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
}
