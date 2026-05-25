using Capsula.Domain.Entities.Carts;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Carts;

public interface ICartItemRepository : IRepository<CartItem, int>, IApplicationService, IScopedService
{
    Task<CartItem> GetCustomerCartItemAsync(int productId, int productSellingUnit, string customerId);
}
