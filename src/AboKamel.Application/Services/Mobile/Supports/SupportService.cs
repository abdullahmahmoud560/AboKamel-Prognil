using AutoMapper;
using Capsula.Application.Contracts.Mobile.Supports;
using Capsula.Application.Dtos.Mobile.Supports;
using Capsula.Domain.Entities.Supports;
using Capsula.Domain.Repositories.Supports;
using Microsoft.Extensions.Logging;

namespace Capsula.Application.Services.Mobile.Supports;

public class SupportService : CrudService<SupportRequestDto, Support, SupportResponseDto, SupportDetailedResponseDto, int>, ISupportService
{
    public SupportService(ISupportRepository repository, IMapper mapper, ILogger<Support> logger) : base(repository, mapper, logger)
    {
    }
}