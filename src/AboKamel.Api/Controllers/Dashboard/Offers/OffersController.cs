using AboKamel.Application.Contracts.Dashboard.Offers;
using AboKamel.Application.Dtos.Dashboard.Offers;
using Capsula.Api.Controllers.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Results;

public class OffersController : DashboardBaseController
{
    private readonly IOfferService _offerService;

    public OffersController(IOfferService offerService)
    {
        _offerService = offerService;
    }

    [HttpGet]
    public async Task<ActionResult<ResultAbstract<IEnumerable<OfferResponseDto>>>> GetAllAsync()
    {
        var result = await _offerService.GetOffersWithDetailsAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultAbstract<OfferResponseDto>>> CreateAsync(OfferRequestDto request)
    {
        var result = await _offerService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultAbstract<OfferResponseDto>>> UpdateAsync(OfferRequestDto request, int id)
    {
        var result = await _offerService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultAbstract<OfferResponseDto>>> DeleteAsync(int id)
    {
        var result = await _offerService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}