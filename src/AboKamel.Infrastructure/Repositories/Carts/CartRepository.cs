using Capsula.Domain.Entities.Carts;
using Capsula.Domain.Repositories.Carts;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Carts;

public class CartRepository : Repository<Cart, int>, ICartRepository
{
    private readonly CapsulaDbContext _context;

    public CartRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Cart> GetCustomerCartAsync(string customerId)
    {
        return await _context.Carts.Where(c => c.CustomerId == customerId).FirstOrDefaultAsync();
    }

    public async Task<Cart> GetCustomerCartDetailsAsync(string customerId)
    {
        return await _context.Carts
            .Where(c => c.CustomerId == customerId)
            .Include(c => c.Customer)
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .Include(c => c.Items)
                .ThenInclude(i => i.ProductSellingUnit)
            .Include(c => c.Prescription)
                .ThenInclude(p => p.VoiceRecords)
            .Include(c => c.Prescription)
                .ThenInclude(p => p.PrescriptionImages)
            .FirstOrDefaultAsync();
    }
}
