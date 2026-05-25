using Capsula.Domain.Entities.Addresses;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Addresses;

public interface IAddressRepository : IRepository<Address, int>, IApplicationService, IScopedService
{
    Task<Address> GetPrimaryAddressAsync(string customerId);
    Task<IEnumerable<Address>> GetCustomerAddressesAsync(string customerId);
    Task<Address> GetCustomerAddressById(string customerId, int addressId);
}