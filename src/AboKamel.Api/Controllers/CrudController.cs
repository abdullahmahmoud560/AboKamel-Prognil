using Capsula.Application.Contracts;
using Capsula.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Entities;

namespace Capsula.Api.Controllers;

public class CrudController<TRequestDto, TEntity, TResponseDto, TDetailedResponseDto, TKey> : ControllerBase
    where TRequestDto : BaseRequestDto
    where TEntity : class, IEntity<TKey>
    where TResponseDto : BaseResponseDto<TKey>
    where TDetailedResponseDto : BaseResponseDto<TKey>
{
    private readonly ICrudService<TRequestDto, TEntity, TResponseDto, TDetailedResponseDto, TKey> _crudService;

    public CrudController(ICrudService<TRequestDto, TEntity, TResponseDto, TDetailedResponseDto, TKey> crudService)
    {
        _crudService = crudService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int page = 1, int NumberOfItems = 12)
    {
        var result = await _crudService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllAsync(TKey id)
    {
        var result = await _crudService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TRequestDto request)
    {
        var result = await _crudService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromBody] TRequestDto request, TKey id)
    {
        var result = await _crudService.UpdateAsync(request, id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(TKey id)
    {
        var result = await _crudService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}