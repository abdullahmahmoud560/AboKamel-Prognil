using AboKamel.Application.Contracts.Dashboard.SellingUnits;
using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using AboKamel.Domain.Entities.SellingUnits;
using AboKamel.Domain.Repositories.SellingUnits;
using AutoMapper;
using Capsula.Application.Services;
using Microsoft.Extensions.Logging;
using Services.Domain.Repositories;

namespace AboKamel.Application.Services.Dashboard.SellingUnits;

public class SellingUnitService : CrudService<SellingUnitRequestDto, SellingUnit, SellingUnitResponseDto, SellingUnitDetailedResponseDto, int>, ISellingUnitService
{
    private readonly ISellingUnitRepository _sellingUnitRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SellingUnit> _logger;

    public SellingUnitService(ISellingUnitRepository repository, IMapper mapper, ILogger<SellingUnit> logger) : base(repository, mapper, logger)
    {
        _sellingUnitRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }
}
