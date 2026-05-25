using AboKamel.Core.Dtos;
using Capsula.Domain.Entities.Users.Customers;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Customers;

public interface ICustomerRepository : IRepository<Customer, string>, IApplicationService, IScopedService
{
    Task<List<CustomerWithRolesDto>> GetAllCustomersWithRolesAsync();
}
