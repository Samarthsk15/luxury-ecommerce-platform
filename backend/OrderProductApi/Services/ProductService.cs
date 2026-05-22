namespace OrderProductApi.Services;
using OrderProductApi.Models;
using OrderProductApi.Repositories;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetAllProductsAsync(int page, int limit, string? category)
    {
        return await _productRepository.GetAllAsync(page, limit, category);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product payload)
    {
        var product = new Product
        {
            Name = payload.Name,
            Description = payload.Description,
            Price = payload.Price,
            InventoryCount = payload.InventoryCount,
            ImageUrl = payload.ImageUrl
        };
        return await _productRepository.AddAsync(product);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product payload)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        product.Name = payload.Name;
        product.Description = payload.Description;
        product.Price = payload.Price;
        product.InventoryCount = payload.InventoryCount;
        product.ImageUrl = payload.ImageUrl;
        
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}
