using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using Capsula.Application.Contracts.Dashboard.Products;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Dtos;
using Services.Core.Results;

namespace Capsula.Api.Controllers.Dashboard.Products;

public class ProductsController : DashboardBaseController
{
    private readonly IProductService _productService;
    private readonly IImageService _imageService;

    public ProductsController(IProductService productService, IImageService imageService)
    {
        _productService = productService;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<ActionResult<ResultAbstract<PagedResultDto<ProductResponseDto>>>> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _productService.GetAllProductsAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultAbstract<ProductDetailedResponseDto>>> GetByIdAsync(int id)
    {
        //var result = await _productService.GetByIdAsync(id);
        var result = await _productService.GetProductDetailsAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> CreateAsync([FromForm] ProductRequestDto request)
    {
        _imageService.ValidateImage(request.ImageFile);

        var result = await _productService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl ?? string.Empty);
        await _imageService.SaveImageAsync(relativePath, request.ImageFile);

        return Ok(result);
    }

    [HttpPost("CreateProductSellingUnits")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> CreateProductSellingUnitsAsync(List<ProductSellingUnitRequestDto> request)
    {
        var result = await _productService.CreateProductSellingUnits(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("UpdateProductSellingUnits")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> UpdateProductSellingUnitsAsync(ProductSellingUnitRequestDto request)
    {
        var result = await _productService.UpdateProductSellingUnit(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> UpdateAsync([FromForm]ProductRequestDto request, int id)
    {
        _imageService.ValidateImage(request.ImageFile);

        var result = await _productService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl ?? string.Empty);
        await _imageService.SaveImageAsync(relativePath, request.ImageFile);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<ProductResponseDto>>> DeleteAsync(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl ?? string.Empty);
        _imageService.DeleteImage(relativePath);

        return Ok(result);
    }

    [HttpGet("LowStock")]
    public async Task<ActionResult<ResultAbstract<List<ProductResponseDto>>>> GetLowStockProductsAsync([FromQuery] int threshold = 10)
    {
        var result = await _productService.GetLowStockProductsAsync(threshold);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("BestSelling")]
    public async Task<ActionResult<ResultAbstract<List<ProductResponseDto>>>> GetBestSellingProductsAsync([FromQuery] int take = 10)
    {
        var result = await _productService.GetBestSellingProductsAsync(take);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("LowStock/Count")]
    public async Task<ActionResult<ResultAbstract<int>>> GetLowStockProductsCountAsync([FromQuery] int threshold = 10)
    {
        var result = await _productService.GetLowStockProductsCountAsync(threshold);
        return Ok(result);
    }

    [HttpGet("BestSelling/Count")]
    public async Task<ActionResult<ResultAbstract<int>>> GetBestSellingProductsCountAsync()
    {
        var result = await _productService.GetBestSellingProductsCountAsync();
        return Ok(result);
    }
}
