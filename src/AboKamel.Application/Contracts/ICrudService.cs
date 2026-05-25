using Capsula.Application.Dtos;
using Services.Core.Entities;
using Services.Core.Results;

namespace Capsula.Application.Contracts;

public interface ICrudService<TRequestDto, TEntity, TResponseDto, TDetailedResponseDto, TKey>
    where TRequestDto : BaseRequestDto
    where TEntity : class, IEntity<TKey>
    where TResponseDto : BaseResponseDto<TKey>
    where TDetailedResponseDto : BaseResponseDto<TKey>
{
    Task<ResultAbstract<IEnumerable<TResponseDto>>> GetAllAsync();
    Task<ResultAbstract<TDetailedResponseDto>> GetByIdAsync(TKey id);
    Task<ResultAbstract<TResponseDto>> CreateAsync(TRequestDto request);
    Task<ResultAbstract<TResponseDto>> UpdateAsync(TRequestDto request, TKey id);
    Task<ResultAbstract<TResponseDto>> DeleteAsync(TKey id);
}