using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using AboKamel.Domain.Entities.SellingUnits;
using Capsula.Application.Contracts;
using Services.Core.DependencyInjection;

namespace AboKamel.Application.Contracts.Dashboard.SellingUnits;

public interface ISellingUnitService : ICrudService<SellingUnitRequestDto, SellingUnit, SellingUnitResponseDto, SellingUnitDetailedResponseDto, int>, IApplicationService, IScopedService
{

}