using Capsula.Domain.Entities.Prescriptions;
using Capsula.Domain.Repositories.Carts;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Carts;

public class PrescriptionRepository : Repository<Prescription, int>, IPrescriptionRepository
{
    private readonly CapsulaDbContext _context;

    public PrescriptionRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Prescription> GetCartPrescription(int cartId)
    {
        return await _context.Prescriptions.Where(p => p.CartId == cartId).FirstOrDefaultAsync();
    }
}
