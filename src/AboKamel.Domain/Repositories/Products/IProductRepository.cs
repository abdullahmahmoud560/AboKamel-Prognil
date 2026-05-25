using Capsula.Domain.Entities.Products;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Products;

public interface IProductRepository : IRepository<Product, int>, IApplicationService, IScopedService
{
    Task<IEnumerable<Product>> SearchProductsAsync(string productName, string productBrand, string productCategory, string productDescription);
    Task<Product> GetProductDetailsAsync(int id);
    Task<List<Product>> GetProductsWithDetailsAsync();
    Task<List<Product>> GetNewestProductsWithDetailsAsync();
    Task<List<Product>> GetLowStockProductsAsync(int threshold = 10);
    Task<List<Product>> GetBestSellingProductsAsync(int take = 10);
    Task<int> GetLowStockProductsCountAsync(int threshold = 10);
    Task<int> GetBestSellingProductsCountAsync();
}
