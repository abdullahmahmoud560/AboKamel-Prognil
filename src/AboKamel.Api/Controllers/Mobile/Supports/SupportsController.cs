using Capsula.Application.Contracts.Mobile.Supports;
using Capsula.Application.Dtos.Mobile.Supports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;

namespace Capsula.Api.Controllers.Mobile.Supports;

public class SupportsController : MobileBaseController
{
    private readonly ISupportService _supportService;

    public SupportsController(ISupportService supportService)
    {
        _supportService = supportService;
    }

    /// <summary>
    /// Creates a new support ticket with optional attachment.
    /// Uses [FromForm] to handle file uploads and form data.
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateSupportAsync([FromForm] SupportRequestDto support)
    {
        var result = await _supportService.CreateAsync(support);

        if (result == null || !result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}