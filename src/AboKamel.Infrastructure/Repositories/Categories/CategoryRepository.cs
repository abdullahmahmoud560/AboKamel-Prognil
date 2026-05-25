using Capsula.Domain.Entities.Categories;
using Capsula.Domain.Repositories.Categories;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Categories;

public class CategoryRepository : Repository<Category, int>, ICategoryRepository
{
    private readonly CapsulaDbContext _context;

    public CategoryRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Category> GetCategoryBrands(int id)
    {
        return await _context.Categories.Include(c => c.Brands).SingleOrDefaultAsync(c => c.Id == id);
    }
}
