using AboKamel.Domain.Entities.SellingUnits;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace AboKamel.Domain.Repositories.SellingUnits;

public interface ISellingUnitRepository : IRepository<SellingUnit, int>, IApplicationService, IScopedService
{

}