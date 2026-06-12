using Capsula.Application.Contracts.Mobile.Products;
using Capsula.Application.Dtos.Mobile.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;

namespace Capsula.Api.Controllers.Mobile.Products;

public class ProductsController : MobileBaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpGet("GetProductsWithFavorite")]
    public async Task<ActionResult<ResultAbstract<List<ProductFavoriteResponseDto>>>> GetProductsWithFavoriteAsync()
    {
        return await _productService.GetAllProductsWithFavoriteAsync();
    }

    [Authorize]
    [HttpGet("GetNewestProducts")]
    public async Task<ActionResult<ResultAbstract<List<ProductFavoriteDetailedResponseDto>>>> GetNewestProductsWithDetailsAsync()
    {
        return await _productService.GetNewestProductsAsync();
    }

    [Authorize]
    [HttpPost("SearchProducts")]
    public async Task<ActionResult<ResultAbstract<List<ProductFavoriteDetailedResponseDto>>>> SearchProductsAsync(ProductSearchRequestDto searchTerm)
    {
        return await _productService.SearchProductsAsync(searchTerm);
    }
}
