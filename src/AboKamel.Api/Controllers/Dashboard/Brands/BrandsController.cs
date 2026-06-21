using Capsula.Application.Contracts.Dashboard.Brands;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Brands;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Dtos;
using Services.Core.Results;

namespace Capsula.Api.Controllers.Dashboard.Brands;

public class BrandsController : DashboardBaseController
{
    private readonly IBrandService _brandService;
    private readonly IImageService _imageService;

    public BrandsController(IBrandService brandService, IImageService imageService)
    {
        _brandService = brandService;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<ActionResult<ResultAbstract<PagedResultDto<BrandResponseDto>>>> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _brandService.GetAllBrandsAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultAbstract<BrandDetailedResponseDto>>> GetByIdAsync(int id)
    {
        var result = await _brandService.GetBrandWithProducts(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultAbstract<BrandResponseDto>>> CreateAsync([FromForm] BrandRequestDto request)
    {
        _imageService.ValidateImage(request.ImageFile);

        var result = await _brandService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl ?? string.Empty);
        await _imageService.SaveImageAsync(relativePath, request.ImageFile);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultAbstract<BrandResponseDto>>> UpdateAsync([FromForm] BrandRequestDto request, int id)
    {
        _imageService.ValidateImage(request.ImageFile);

        var result = await _brandService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl ?? string.Empty);
        await _imageService.SaveImageAsync(relativePath, request.ImageFile);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultAbstract<BrandResponseDto>>> DeleteAsync(int id)
    {
        var result = await _brandService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl ?? string.Empty);
        _imageService.DeleteImage(relativePath);

        return Ok(result);
    }
}
