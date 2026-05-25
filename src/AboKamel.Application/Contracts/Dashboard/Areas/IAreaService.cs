using AboKamel.Application.Dtos.Dashboard.Areas;
using AboKamel.Domain.Entities.Areas;
using Capsula.Application.Contracts;
using Services.Core.DependencyInjection;

namespace AboKamel.Application.Contracts.Dashboard.Areas;

public interface IAreaService : ICrudService<AreaRequestDto, Area, AreaResponseDto, AreaDetailedResponseDto, int>, IApplicationService, IScopedService
{

}