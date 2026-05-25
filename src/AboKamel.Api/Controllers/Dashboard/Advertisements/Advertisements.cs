using AboKamel.Application.Contracts.Dashboard.Advertisements;
using AboKamel.Application.Dtos.Dashboard.Advertisements;
using Capsula.Api.Controllers.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace AboKamel.Api.Controllers.Dashboard.Advertisements;

public class AdvertisementsController : DashboardBaseController
{
    private readonly IAdvertisementService _advertisementService;

    public AdvertisementsController(IAdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateAdvertisementRequest request)
    {
        var id = await _advertisementService.CreateAsync(request);
        return Ok(new { Id = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateAdvertisementRequest request)
    {
        await _advertisementService.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _advertisementService.DeleteAsync(id);
        return Ok();
    }
}