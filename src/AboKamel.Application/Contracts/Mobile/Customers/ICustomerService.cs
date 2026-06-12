using AboKamel.Application.Dtos.Authentication.Users.Customers;
using AboKamel.Core.Dtos;
using Capsula.Application.Dtos.Authentication.Users.Customers;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Customers;

public interface ICustomerService : IApplicationService, IScopedService
{
    Task<ResultAbstract<CustomerResponseDto>> RegisterCustomerAsync(RegisterCustomerRequestDto request);
    Task<ResultAbstract<List<CustomerWithRolesDto>>> GetAllCustomerAsync();
    Task<ResultAbstract<CustomerResponseDto>> UpdateCustomerAsync(string userId, UpdateCustomerRequestDto dto);
    // Backward compatibility overload for Dashboard
    Task<ResultAbstract<CustomerResponseDto>> UpdateCustomerAsync(CustomerRequestDto request, string? customerId = null);
    Task<ResultAbstract<bool>> EditCustomerStatusAsync(string customerId, bool active);
    Task<Result> DeleteCustomerAccountAsync(string userId);
}