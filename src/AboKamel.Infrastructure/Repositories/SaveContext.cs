using Services.Domain.Repositories;
using Services.Infrastructure.DbContexts;

namespace Services.Infrastructure.Repositories;

///<inheritdoc/>
public class SaveContext : ISaveContext
{
    private readonly CapsulaDbContext _dbContext;

    public SaveContext(CapsulaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    ///<inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
