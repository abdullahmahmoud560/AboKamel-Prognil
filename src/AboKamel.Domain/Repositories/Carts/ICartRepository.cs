using Capsula.Domain.Entities.Carts;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Carts;

public interface ICartRepository : IRepository<Cart, int>, IApplicationService, IScopedService
{
    Task<Cart> GetCustomerCartAsync(string customerId);
    Task<Cart> GetCustomerCartDetailsAsync(string customerId);
}
