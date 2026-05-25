using AboKamel.Core.Dtos;
using Capsula.Application.Dtos.Authentication.Users.Customers;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Customers;

public interface ICustomerService : IApplicationService, IScopedService
{
    Task<ResultAbstract<CustomerResponseDto>> RegisterCustomerAsync(CustomerRequestDto request);
    Task<ResultAbstract<List<CustomerWithRolesDto>>> GetAllCustomerAsync();
    Task<ResultAbstract<CustomerResponseDto>> UpdateCustomerAsync(CustomerRequestDto request, string customerId);
    Task<ResultAbstract<bool>> EditCustomerStatusAsync(string customerId, bool active);
}