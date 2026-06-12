using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Contracts.Mobile.Supports;
using Capsula.Application.Dtos.Mobile.Supports;
using Capsula.Domain.Entities.Supports;
using Capsula.Domain.Repositories.Supports;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

namespace Capsula.Application.Services.Mobile.Supports;

public class SupportService : CrudService<SupportRequestDto, Support, SupportResponseDto, SupportDetailedResponseDto, int>, ISupportService
{
    private readonly IImageService _imageService;
    private readonly ISupportRepository _repository;
    private readonly IMapper _mapper;

    public SupportService(
        ISupportRepository repository,
        IMapper mapper,
        ILogger<Support> logger,
        IImageService imageService) : base(repository, mapper, logger)
    {
        _repository = repository;
        _mapper = mapper;
        _imageService = imageService;
    }

    public override async Task<ResultAbstract<SupportResponseDto>> CreateAsync(SupportRequestDto request)
    {
        var entity = _mapper.Map<Support>(request);

        if (request.Attachment != null)
        {
            var path = _imageService.GetImageRelativePath(request.Attachment);
            await _imageService.SaveImageAsync(path, request.Attachment);
            entity.AttachmentPath = path;
        }

        bool isRecordAdded = await _repository.AddAsync(entity);

        if (!isRecordAdded)
        {
            return Result.Error("Could not create support ticket.");
        }

        return Result.Success(_mapper.Map<SupportResponseDto>(entity));
    }
}