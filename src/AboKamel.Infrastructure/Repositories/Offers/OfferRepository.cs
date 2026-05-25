using AboKamel.Domain.Entities.Offers;
using AboKamel.Domain.Repositories.Offers;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace AboKamel.Infrastructure.Repositories.Offers;

public class OfferRepository : Repository<Offer, int>, IOfferRepository
{
    CapsulaDbContext _context;

    public OfferRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Offer>> GetOfferWithDetailsAsync()
    {
        return await _context.Offers
            .Include( o => o.Product)
                .ThenInclude(p => p.ProductSellingUnits)
                .ThenInclude(s => s.SellingUnit)
            .Include(o => o.Area)
            .ToListAsync();
    }
}