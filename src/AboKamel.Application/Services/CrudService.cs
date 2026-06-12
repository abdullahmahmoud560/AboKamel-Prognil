using AutoMapper;
using Capsula.Application.Contracts;
using Capsula.Application.Dtos;
using Microsoft.Extensions.Logging;
using Services.Core.Entities;
using Services.Core.Results;
using Services.Domain.Repositories;

namespace Capsula.Application.Services;

public class CrudService<TRequestDto, TEntity, TResponseDto, TDetailedResponseDto, TKey> : ICrudService<TRequestDto, TEntity, TResponseDto, TDetailedResponseDto, TKey>
    where TRequestDto : BaseRequestDto
    where TEntity : class, IEntity<TKey>
    where TResponseDto : BaseResponseDto<TKey>
    where TDetailedResponseDto : BaseResponseDto<TKey>
{
    private readonly IRepository<TEntity, TKey> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TEntity> _logger;

    public CrudService(IRepository<TEntity, TKey> repository, IMapper mapper, ILogger<TEntity> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public virtual async Task<ResultAbstract<IEnumerable<TResponseDto>>> GetAllAsync()
    {
        var records = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<TResponseDto>>(records);
        return Result.Success(result);
    }

    public virtual async Task<ResultAbstract<TDetailedResponseDto>> GetByIdAsync(TKey id)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record == null)
        {
            _logger.LogWarning("السجل رقم {Id} غير موجود", id);
            return Result.Error("لم يتم العثور على السجل");
        }
        return Result.Success(_mapper.Map<TDetailedResponseDto>(record));
    }

    public virtual async Task<ResultAbstract<TResponseDto>> CreateAsync(TRequestDto request)
    {
        var record = _mapper.Map<TEntity>(request);
        bool isRecordAdded = await _repository.AddAsync(record);

        if (!isRecordAdded)
        {
            _logger.LogError($"لم يتم إنشاء السجل");
            return Result.Error("لم يتم إنشاء السجل");
        }

        _logger.LogInformation("تم إضافة السجل بنجاح برقم {Id}", record.Id);
        return Result.Success(_mapper.Map<TResponseDto>(record));
    }

    public virtual async Task<ResultAbstract<TResponseDto>> UpdateAsync(TRequestDto request, TKey id)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record == null)
        {
            _logger.LogWarning("فشل التحديث. السجل رقم {Id} غير موجود", id);
            return Result.Error("لم يتم العثور على السجل");
        }

        _mapper.Map(request, record);
        bool isRecordUpdated = await _repository.EditAsync(record);

        if (!isRecordUpdated)
        {
            _logger.LogError("لم يتم تحديث السجل");
            return Result.Error($"لم يتم تحديث السجل");
        }

        _logger.LogInformation("تم تحديث السجل رقم {Id} بنجاح", id);
        return Result.Success(_mapper.Map<TResponseDto>(record));
    }

    public virtual async Task<ResultAbstract<TResponseDto>> DeleteAsync(TKey id)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record == null)
        {
            _logger.LogWarning("فشل الحذف. السجل رقم {Id} غير موجود", id);
            return Result.Error("لم يتم العثور على السجل");
        }

        bool isRecordDeleted = await _repository.DeleteAsync(record);

        if (!isRecordDeleted)
        {
            _logger.LogError("لم يتم حذف السجل");
            return Result.Error($"لم يتم حذف السجل");
        }

        _logger.LogInformation("تم حذف السجل رقم {Id} بنجاح", id);
        return Result.Success(_mapper.Map<TResponseDto>(record));
    }
}
