using Capsula.Domain.Entities.Brands;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Brands;

public interface IBrandRepository : IRepository<Brand, int>, IApplicationService, IScopedService
{
    Task<Brand> GetBrandWithProductsAsync(int id);
}
