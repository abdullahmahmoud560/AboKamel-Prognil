using Capsula.Application.Contracts.Mobile.Addresses;
using Capsula.Application.Dtos.Mobile.Addresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;

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
        return await _addressService.GetCustomerAddressesAsync();
    }

    [HttpGet("GetPrimaryCustomerAddress")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> GetPrimaryAddressAsync()
    {
        return await _addressService.GetPrimaryAddressAsync();
    }

    [HttpPatch("MarkAddressAsPrimary/{addressId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> MarkAddressAsPrimaryAsync(int addressId)
    {
        return await _addressService.MarkAddressAsPrimaryAsync(addressId);
    }

    [HttpPost("CreateCustomerAddress")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> CreateCustomerAddressAsync(AddressRequestDto request)
    {
        return await _addressService.CreateCustomerAddressAsync(request);
    }

    [HttpPut("UpdateCustomerAddress/{addressId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> UpdateCustomerAddressAsync(AddressNotPrimaryRequestDto request, int addressId)
    {
        return await _addressService.UpdateCustomerAddressAsync(request, addressId);
    }

    [HttpDelete("DeleteCustomerAddress/{addressId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<AddressResponseDto>>> DeleteCustomerAddressAsync(int addressId)
    {
        return await _addressService.DeleteCustomerAddressAsync(addressId);
    }
}
