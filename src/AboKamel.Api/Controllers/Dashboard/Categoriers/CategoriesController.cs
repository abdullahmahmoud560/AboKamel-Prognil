using Capsula.Application.Contracts.Dashboard.Categories;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Results;

namespace Capsula.Api.Controllers.Dashboard.Categoriers;

[Authorize]
public class CategoriesController : DashboardBaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IImageService _imageService;

    public CategoriesController(ICategoryService categoryService, IImageService imageService)
    {
        _categoryService = categoryService;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<ActionResult<ResultAbstract<IEnumerable<CategoryResponseDto>>>> GetAllAsync()
    {
        var result = await _categoryService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultAbstract<CategoryDetailedResponseDto>>> GetByIdAsync(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("/category/brands/{id}")]
    public async Task<ActionResult<ResultAbstract<CategoryDetailedResponseDto>>> GetCAtegoryBrandsAsync(int id)
    {
        var result = await _categoryService.GetCategoryBrandsAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultAbstract<CategoryResponseDto>>> CreateAsync([FromForm] CategoryRequestDto request)
    {
        _imageService.ValidateImage(request.ImageFile);

        var result = await _categoryService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl);
        await _imageService.SaveImageAsync(relativePath, request.ImageFile);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultAbstract<CategoryResponseDto>>> UpdateAsync([FromForm] CategoryRequestDto request, int id)
    {
        _imageService.ValidateImage(request.ImageFile);

        var result = await _categoryService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl);
        await _imageService.SaveImageAsync(relativePath, request.ImageFile);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultAbstract<CategoryResponseDto>>> DeleteAsync(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        var relativePath = _imageService.ExtractImagePath(result.Value.ImageUrl);
        _imageService.DeleteImage(relativePath);

        return Ok(result);
    }
}