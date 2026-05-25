using Capsula.Domain.Entities.Products;
using Capsula.Domain.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Products;

public class ProductRepository : Repository<Product, int>, IProductRepository
{
    private readonly CapsulaDbContext _context;

    public ProductRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetLowStockProductsAsync(int threshold = 10)
    {
        return await _context.Products
            .Where(p => p.ProductSellingUnits.Any(psu => psu.Quantity <= threshold))
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Area)
            .Include(p => p.ProductSellingUnits)
            .ToListAsync();
    }

    public async Task<List<Product>> GetNewestProductsWithDetailsAsync()
    {
        return await _context.Products
            .Include(p => p.ProductSellingUnits)
                .ThenInclude(u => u.SellingUnit)
                .OrderByDescending(p => p.CreatedDate)
                .Take(6)
            .ToListAsync();
    }

    public async Task<Product> GetProductDetailsAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Area)
            .Include(p => p.ProductSellingUnits)
                .ThenInclude(u => u.SellingUnit)
            .Where(p => p.Id == id).SingleOrDefaultAsync();
    }

    public async Task<List<Product>> GetProductsWithDetailsAsync()
    {
        return await _context.Products
            .Include(p => p.ProductSellingUnits)
                .ThenInclude(u => u.SellingUnit)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(
        string productName,
        string productBrand,
        string productCategory,
        string productDescription)
    {
        var query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(productName))
        {
            query = query.Where(p => p.Name.Contains(productName));
        }

        if (!string.IsNullOrWhiteSpace(productBrand))
        {
            query = query.Where(p => p.Brand.Name.Contains(productBrand));
        }

        if (!string.IsNullOrWhiteSpace(productCategory))
        {
            query = query.Where(p => p.Category.Name.Contains(productCategory));
        }

        if (!string.IsNullOrWhiteSpace(productDescription))
        {
            query = query.Where(p => p.Description.Contains(productDescription));
        }

        return await query.ToListAsync();
    }

    public async Task<List<Product>> GetBestSellingProductsAsync(int take = 10)
    {
        // 1️⃣ Get best-selling product IDs
        var bestSellingProductIds = await _context.OrderItems
            .GroupBy(oi => oi.ProductId)
            .OrderByDescending(g => g.Sum(x => x.Quantity))
            .Select(g => g.Key)
            .Take(take)
            .ToListAsync();

        // 2️⃣ Load products with Includes
        return await _context.Products
            .Where(p => bestSellingProductIds.Contains(p.Id))
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Area)
            .Include(p => p.ProductSellingUnits)
            .ToListAsync();
    }

    public async Task<int> GetLowStockProductsCountAsync(int threshold = 10)
    {
        return await _context.Products
            .Where(p => p.ProductSellingUnits.Any(psu => psu.Quantity <= threshold))
            .CountAsync();
    }

    public async Task<int> GetBestSellingProductsCountAsync()
    {
        return await _context.OrderItems
            .Select(oi => oi.ProductId)
            .Distinct()
            .CountAsync();
    }
}
