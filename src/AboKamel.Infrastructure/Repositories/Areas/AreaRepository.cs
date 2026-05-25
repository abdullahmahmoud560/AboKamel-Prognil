using AboKamel.Domain.Entities.Areas;
using AboKamel.Domain.Repositories.Areas;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace AboKamel.Infrastructure.Repositories.Areas;

public class AreaRepository : Repository<Area, int>, IAreaRepository
{
    public AreaRepository(CapsulaDbContext context) : base(context)
    {
    }
}