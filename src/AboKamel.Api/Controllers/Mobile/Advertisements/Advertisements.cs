using AboKamel.Application.Contracts.Dashboard.Advertisements;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;

namespace AboKamel.Api.Controllers.Mobile.Advertisements;

public class AdvertisementsController : MobileBaseController
{
    private readonly IAdvertisementService _advertisementService;

    public AdvertisementsController(IAdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpGet("search/{name}")]
    public async Task<IActionResult> Search(string name)
    {
        var result = await _advertisementService.SearchByNameAsync(name);
        return Ok(result);
    }
}