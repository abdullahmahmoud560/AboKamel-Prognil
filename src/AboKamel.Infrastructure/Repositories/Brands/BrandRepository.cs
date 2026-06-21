using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Repositories.Brands;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Brands;

public class BrandRepository : Repository<Brand, int>, IBrandRepository
{
    private readonly CapsulaDbContext _context;

    public BrandRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Brand> GetBrandWithProductsAsync(int id)
    {
        return await _context.Brands.Include(b => b.Products).FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<(List<Brand> Brands, int TotalCount)> GetPagedBrandsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Brands.CountAsync();
        var brands = await _context.Brands
            .OrderByDescending(b => b.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (brands, totalCount);
    }
}
