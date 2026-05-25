using Capsula.Application.Contracts.Mobile.Supports;
using Capsula.Application.Dtos.Mobile.Supports;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;

namespace Capsula.Api.Controllers.Mobile.Supports;

public class SupportsController : MobileBaseController
{
    private readonly ISupportService _supportService;

    public SupportsController(ISupportService supportService)
    {
        _supportService = supportService;
    }

    [HttpPost]
    public async Task<ActionResult<ResultAbstract<SupportResponseDto>>> CreateSupportAsync(SupportRequestDto support)
    {
        return await _supportService.CreateAsync(support);
    }
}