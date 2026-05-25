using AboKamel.Domain.Entities.SellingUnits;
using AboKamel.Domain.Repositories.SellingUnits;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace AboKamel.Infrastructure.Repositories.SellingUnits;

public class SellingUnitRepository : Repository<SellingUnit, int>, ISellingUnitRepository
{
    public SellingUnitRepository(CapsulaDbContext context) : base(context)
    {
    }
}