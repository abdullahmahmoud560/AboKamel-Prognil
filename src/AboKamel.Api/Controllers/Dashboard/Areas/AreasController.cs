using AboKamel.Application.Contracts.Dashboard.Areas;
using AboKamel.Application.Dtos.Dashboard.Areas;
using Capsula.Api.Controllers.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Results;

namespace AboKamel.Api.Controllers.Dashboard.Areas;

public class AreasController : DashboardBaseController
{
    private readonly IAreaService _areaService;

    public AreasController(IAreaService areaService)
    {
        _areaService = areaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ResultAbstract<IEnumerable<AreaResponseDto>>>> GetAllAsync()
    {
        var result = await _areaService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultAbstract<AreaResponseDto>>> CreateAsync(AreaRequestDto request)
    {
        var result = await _areaService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultAbstract<AreaResponseDto>>> UpdateAsync(AreaRequestDto request, int id)
    {
        var result = await _areaService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultAbstract<AreaResponseDto>>> DeleteAsync(int id)
    {
        var result = await _areaService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}