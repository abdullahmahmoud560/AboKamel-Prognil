using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Application.Dtos.Mobile.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;

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
        return await _cartItemService.AddItemToCartAsync(request);
    }

    [HttpPut("UpdateCartItemQuantity")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> UpdateCartItemQuantityAsync(CartItemRequestDto request)
    {
        var result = await _cartItemService.UpdateCartItemQuantityAsync(request);

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
        var result = await _cartItemService.DeleteCartItemAsync(productId, productSellingUnitId);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
