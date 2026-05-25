using AboKamel.Domain.Entities.SellingUnits;
using AboKamel.Domain.Repositories.SellingUnits;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace AboKamel.Infrastructure.Repositories.SellingUnits;

public class ProductSellingUnitRepository : Repository<ProductSellingUnit, int>, IProductSellingUnitRepository
{
    public ProductSellingUnitRepository(CapsulaDbContext context) : base(context)
    {
    }
}
