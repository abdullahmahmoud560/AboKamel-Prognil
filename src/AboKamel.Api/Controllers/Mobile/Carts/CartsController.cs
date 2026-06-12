using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Prescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;

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
        return await _cartService.GetCustomerCartDetailsAsync();
    }

    [HttpPost("AddPrescriptionImage")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<bool>>> AddPrescriptionImageAsync([FromForm] FileRequestDto file)
    {
        return await _cartService.AddPrescriptionImageToCartAsync(file.File);
    }

    [HttpPost("AddVoiceRecord")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<bool>>> AddVoiceRecordAsync([FromForm] FileRequestDto file)
    {
        return await _cartService.AddPrescriptionVoiceRecordToCartAsync(file.File);
    }
}
