using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Application.Dtos.Mobile.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Api.Controllers.Mobile.Carts;

public class CartItemsController : MobileBaseController
{
    private readonly ICartItemService _cartItemService;

    public CartItemsController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpPost("AddProductToCart")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<CartItemResponseDto>>> AddItemToCartAsync(CartItemRequestDto request)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _cartItemService.AddItemToCartAsync(request, customerId);
    }

    [HttpPut("UpdateCartItemQuantity")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> UpdateCartItemQuantityAsync(CartItemRequestDto request)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _cartItemService.UpdateCartItemQuantityAsync(request, customerId);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("DeleteCartItem/productId/{productId}/productSellingUnitId/{productSellingUnitId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> DeleteAsync(int productId, int productSellingUnitId)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _cartItemService.DeleteCartItemAsync(productId, productSellingUnitId, customerId);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
