using Capsula.Domain.Entities.Addresses;
using Capsula.Domain.Repositories.Addresses;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Addresses;

public class AddressRepository : Repository<Address, int>, IAddressRepository
{
    private readonly CapsulaDbContext _context;

    public AddressRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Address>> GetCustomerAddressesAsync(string customerId)
    {
        return await _context.Addresses.Where(a => a.CustomerId == customerId).ToListAsync();
    }

    public async Task<Address> GetPrimaryAddressAsync(string customerId)
    {
        return await _context.Addresses.Where(a => a.CustomerId == customerId && a.IsPrimary).FirstOrDefaultAsync();
    }

    public async Task<Address> GetCustomerAddressById(string customerId, int addressId)
    {
        return await _context.Addresses.Where(a => a.CustomerId == customerId && a.Id == addressId).FirstOrDefaultAsync();
    }
}