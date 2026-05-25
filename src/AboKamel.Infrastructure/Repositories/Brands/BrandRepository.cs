using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Repositories.Brands;
using Dapper;
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
}
