using Capsula.Application.Contracts.Mobile.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using System.Security.Claims;

namespace Capsula.Api.Controllers.Mobile;

public class PaymentsController : MobileBaseController
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("CreatePaymentIntention")]
    [Authorize]
    public async Task<ActionResult<string>> CreaatePaymentIntentionAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _paymentService.CreatePaymentIntentionAsync(userId);
    }
}
