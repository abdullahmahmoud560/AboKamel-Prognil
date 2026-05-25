using Capsula.Domain.Entities.Products;
using Capsula.Domain.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Products;

public class FavoriteRepository : Repository<Favorite, int>, IFavoriteRepository
{
    private readonly CapsulaDbContext _context;

    public FavoriteRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(string userId)
    {
        return await _context.Favorites.Include(f => f.Product).Where(f => f.UserId == userId).ToListAsync();
    }

    public async Task<Favorite> GetFavoriteByProductIdAsync(string userId, int productId)
    {
        return await _context.Favorites.Where(f => f.UserId == userId && f.ProductId == productId).FirstOrDefaultAsync();
    }
}