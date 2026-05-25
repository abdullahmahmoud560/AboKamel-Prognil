using Capsula.Domain.Entities.Products;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Products;

public interface IFavoriteRepository : IRepository<Favorite, int>, IApplicationService, IScopedService
{
    Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(string userId);
    Task<Favorite> GetFavoriteByProductIdAsync(string userId, int productId);
}