using Capsula.Domain.Entities.Carts;
using Capsula.Domain.Repositories.Carts;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Carts;

public class CartItemRepository : Repository<CartItem, int>, ICartItemRepository
{
    private readonly CapsulaDbContext _context;

    public CartItemRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CartItem> GetCustomerCartItemAsync(int productId, int productSellingUnitId, string customerId)
    {
        return await _context.CartItems.Where(ct => ct.ProductId == productId && ct.ProductSellingUnitId == productSellingUnitId && ct.Cart.CustomerId == customerId).FirstOrDefaultAsync();
    }
}
