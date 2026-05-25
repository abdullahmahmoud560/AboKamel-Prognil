using Capsula.Application.Contracts.Mobile.Products;
using Capsula.Application.Dtos.Mobile.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Helpers.Roles;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Api.Controllers.Mobile.Products;

[Authorize]
public class FavoritesController : MobileBaseController
{
    private readonly IFavoriteService _favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    // 1. Get favorites (optionally by user)
    [HttpGet("GetCustomerFavorites")]
    [Authorize(Roles = RoleName.Customer)]
    public async Task<ActionResult<ResultAbstract<FavoriteResponseDto>>> GetFavorites()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var favorites = await _favoriteService.GetAllFavoritesByUserIdAsync(userId);
        return Ok(favorites);
    }

    [HttpPost("CreateCustomerFavorite")]
    [Authorize(Roles = RoleName.Customer)]
    public async Task<ActionResult<ResultAbstract<FavoriteResponseDto>>> AddFavorite([FromBody] FavoriteRequestDto dto)
    {
        dto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _favoriteService.CreateAsync(dto);
        return result;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = RoleName.Customer)]
    public async Task<ActionResult<ResultAbstract<bool>>> DeleteFavorite(int id)
    {
        var favoriteRequestDto = new FavoriteRequestDto
        {
            ProductId = id,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        };

        return await _favoriteService.DeleteFavoriteByProductIdAsync(favoriteRequestDto);
    }
}