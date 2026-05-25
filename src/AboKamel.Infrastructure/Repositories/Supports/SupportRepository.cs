using Capsula.Domain.Entities.Supports;
using Capsula.Domain.Repositories.Supports;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Supports;

public class SupportRepository : Repository<Support, int>, ISupportRepository
{
    public SupportRepository(CapsulaDbContext context) : base(context)
    {
    }
}