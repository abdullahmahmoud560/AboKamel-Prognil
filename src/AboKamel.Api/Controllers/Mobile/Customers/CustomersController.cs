using AboKamel.Application.Dtos.Authentication.Users.Customers;
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
    [AllowAnonymous] // Explicitly allow anonymous access since it's registration
    public async Task<ActionResult<ResultAbstract<CustomerResponseDto>>> RegisterCustomer(RegisterCustomerRequestDto customerRequest)
    {
        return await _customerService.RegisterCustomerAsync(customerRequest);
    }

    [HttpPut("UpdateCustomer")]
    [Authorize]
    public async Task<ActionResult<ResultAbstract<CustomerResponseDto>>> UpdateCustomer(UpdateCustomerRequestDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(Result.Error("User not authenticated."));
        }

        return await _customerService.UpdateCustomerAsync(userId, request);
    }

    [HttpDelete("DeleteAccount")]
    [Authorize]
    public async Task<ActionResult<Result>> DeleteAccount()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(Result.Error("User not authenticated."));
        }

        return await _customerService.DeleteCustomerAccountAsync(userId);
    }
}
