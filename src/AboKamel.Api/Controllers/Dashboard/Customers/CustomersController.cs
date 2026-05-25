using Capsula.Application.Contracts.Mobile.Customers;
using Capsula.Application.Dtos.Authentication.Users.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Helpers.Roles;
using Services.Core.Results;

namespace Capsula.Api.Controllers.Dashboard.Customers;

public class CustomersController : DashboardBaseController
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPut("UpdateCustomer/customer/{customerId}")]
    [Authorize(Roles = RoleName.SuperAdmin)]
    public async Task<ActionResult<ResultAbstract<CustomerResponseDto>>> UpdateCustomerAsync(CustomerRequestDto customerRequest, string customerId)
    {
        return await _customerService.UpdateCustomerAsync(customerRequest, customerId);
    }

    [HttpPatch("EditCustomerStatus/customer/{customerId}/status/{status}")]
    [Authorize(Roles = RoleName.SuperAdmin)]
    public async Task<ActionResult<ResultAbstract<bool>>> EditCustomerStatusAsync(string customerId, bool status)
    {
        return await _customerService.EditCustomerStatusAsync(customerId, status);
    }
}
