using Capsula.Application.Contracts.Mobile.Addresses;
using Capsula.Application.Dtos.Mobile.Addresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Api.Controllers.Mobile.Addresses;

public class AddressesController : MobileBaseController
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("GetCustomerAddresses")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<IEnumerable<AddressResponseDto>>>> GetCustomerAddressesAsync()
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _addressService.GetCustomerAddressesAsync(customerId);
    }

    [HttpGet("GetPrimaryCustomerAddress")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> GetPrimaryAddressAsync()
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _addressService.GetPrimaryAddressAsync(customerId);
    }

    [HttpPatch("MarkAddressAsPrimary/{addressId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> MarkAddressAsPrimaryAsync(int addressId)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _addressService.MarkAddressAsPrimaryAsync(customerId, addressId);
    }

    [HttpPost("CreateCustomerAddress")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> CreateCustomerAddressAsync(AddressRequestDto request)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _addressService.CreateCustomerAddressAsync(request, customerId);
    }

    [HttpPut("UpdateCustomerAddress/{addressId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> UpdateCustomerAddressAsync(AddressNotPrimaryRequestDto request, int addressId)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _addressService.UpdateCustomerAddressAsync(request, customerId, addressId);
    }

    [HttpDelete("DeleteCustomerAddress/{addressId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> DeleteCustomerAddressAsync(int addressId)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _addressService.DeleteCustomerAddressAsync(customerId, addressId);
    }
}