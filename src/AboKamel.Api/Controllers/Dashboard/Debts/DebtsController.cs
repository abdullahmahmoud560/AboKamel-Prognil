using AboKamel.Application.Contracts.Dashboard.Debts;
using AboKamel.Application.Dtos.Dashboard.Debts;
using Capsula.Api.Controllers.Dashboard;
using Capsula.Application.Contracts.Dashboard.Brands;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Helpers.Roles;
using Services.Core.Results;

namespace AboKamel.Api.Controllers.Dashboard.Debts;

[Authorize(Roles = RoleName.SuperAdmin)]
public class DebtsController : DashboardBaseController
{
    private readonly IDebtService _debtService;

    public DebtsController(IDebtService debtService)
    {
        _debtService = debtService;
    }

    [HttpGet("GetAllDebitUsers")]
    public async Task<ActionResult<ResultAbstract<List<DebtResponseDto>>>> GetDebitUsersAsync()
    {
        var result = await _debtService.GetDebitUsersAsync();

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("GetUserBalance/user/{userId}")]
    public async Task<ActionResult<ResultAbstract<List<DebtResponseDto>>>> GetUserBalanaceAsync(string userId)
    {
        var result = await _debtService.GetUserBalanceAsync(userId);

        return Ok(result);
    }

    [HttpGet("GetAllCreditUsers")]
    public async Task<ActionResult<ResultAbstract<List<DebtResponseDto>>>> GetCreditsersAsync()
    {
        var result = await _debtService.GetCreditUsersAsync();

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultAbstract<DebtDetailedResponseDto>>> GetByIdAsync(int id)
    {
        var result = await _debtService.GetDebitDetailsAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultAbstract<DebtResponseDto>>> CreateAsync(DebtRequestDto request)
    {
        var result = await _debtService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultAbstract<DebtResponseDto>>> UpdateAsync(DebtRequestDto request, int id)
    {
        var result = await _debtService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultAbstract<DebtResponseDto>>> DeleteAsync(int id)
    {
        var result = await _debtService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
