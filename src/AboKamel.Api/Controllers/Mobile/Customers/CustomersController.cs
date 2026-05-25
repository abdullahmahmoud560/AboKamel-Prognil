using AboKamel.Core.Dtos;
using Capsula.Application.Contracts.Mobile.Customers;
using Capsula.Application.Dtos.Authentication.Users.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Helpers.Roles;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Api.Controllers.Mobile.Customers;

public class CustomersController : MobileBaseController
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("GetAllCustomers")]
    [Authorize(Roles = RoleName.SuperAdmin)]
    public async Task<ActionResult<ResultAbstract<List<CustomerWithRolesDto>>>> GetAllCustomerAsync()
    {
        return await _customerService.GetAllCustomerAsync();
    }

    [HttpPost("RegisterCustomer")]
    public async Task<ActionResult<ResultAbstract<CustomerResponseDto>>> RegisterCustmer(CustomerRequestDto customerRequest)
    {
        return await _customerService.RegisterCustomerAsync(customerRequest);
    }

    [HttpPut("UpdateCustomer")]
    [Authorize(Roles = RoleName.Customer)]
    public async Task<ActionResult<ResultAbstract<CustomerResponseDto>>> UpdateCustomerAsync(CustomerRequestDto customerRequest)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _customerService.UpdateCustomerAsync(customerRequest, customerId);
    }
}
