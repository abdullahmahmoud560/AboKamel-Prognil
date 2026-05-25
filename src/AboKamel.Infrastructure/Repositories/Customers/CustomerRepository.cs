using AboKamel.Core.Dtos;
using Capsula.Domain.Entities.Users.Customers;
using Capsula.Domain.Repositories.Customers;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace Capsula.Infrastructure.Repositories.Customers;

public class CustomerRepository : Repository<Customer, string>, ICustomerRepository
{
    private readonly CapsulaDbContext _context;

    public CustomerRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<CustomerWithRolesDto>> GetAllCustomersWithRolesAsync()
    {
        return await _context.Customers
            .Select(c => new CustomerWithRolesDto
            {
                Id = c.Id,
                UserName = c.UserName,
                Email = c.Email,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Active = c.Active,

                Roles = (
                    from ur in _context.UserRoles
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where ur.UserId == c.Id
                    select r.Name
                ).ToList()
            })
            .ToListAsync();
    }
}
