using AboKamel.Application.Contracts.Dashboard.Areas;
using AboKamel.Application.Dtos.Dashboard.Areas;
using AboKamel.Domain.Entities.Areas;
using AboKamel.Domain.Repositories.Areas;
using AutoMapper;
using Capsula.Application.Services;
using Microsoft.Extensions.Logging;
using Services.Domain.Repositories;

namespace AboKamel.Application.Services.Dashboard.Areas;

public class AreaService : CrudService<AreaRequestDto, Area, AreaResponseDto, AreaDetailedResponseDto, int>, IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Area> _logger;

    public AreaService(IAreaRepository repository, IMapper mapper, ILogger<Area> logger) : base(repository, mapper, logger)
    {
        _areaRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }


}
