using AboKamel.Application.Contracts.Dashboard.SellingUnits;
using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using Capsula.Api.Controllers.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Results;

namespace AboKamel.Api.Controllers.Dashboard.SellingUnits;

public class SellingUnitsController : DashboardBaseController
{
    private readonly ISellingUnitService _sellingUnitService;

    public SellingUnitsController(ISellingUnitService sellingUnitService)
    {
        _sellingUnitService = sellingUnitService;
    }

    [HttpGet]
    public async Task<ActionResult<ResultAbstract<IEnumerable<SellingUnitResponseDto>>>> GetAllAsync()
    {
        var result = await _sellingUnitService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultAbstract<SellingUnitResponseDto>>> CreateAsync(SellingUnitRequestDto request)
    {
        var result = await _sellingUnitService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultAbstract<SellingUnitResponseDto>>> UpdateAsync(SellingUnitRequestDto request, int id)
    {
        var result = await _sellingUnitService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultAbstract<SellingUnitResponseDto>>> DeleteAsync(int id)
    {
        var result = await _sellingUnitService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}