using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Prescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Api.Controllers.Mobile.Carts;

public class CartsController : MobileBaseController
{
    private readonly ICartService _cartService;

    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("GetCustomerCartDetails")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<CartDetailedResponseDto>>> GetCartDetailsAsync()
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _cartService.GetCustomerCartDetailsAsync(customerId);
    }

    [HttpPost("AddPrescriptionImage")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<bool>>> AddPrescriptionImageAsync([FromForm] FileRequestDto file)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _cartService.AddPrescriptionImageToCartAsync(customerId, file.File);
    }

    [HttpPost("AddVoiceRecord")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<bool>>> AddVoiceRecordAsync([FromForm] FileRequestDto file)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _cartService.AddPrescriptionVoiceRecordToCartAsync(customerId, file.File);
    }
}
