using AboKamel.Domain.Entities.Areas;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace AboKamel.Domain.Repositories.Areas;

public interface IAreaRepository : IRepository<Area, int>, IApplicationService, IScopedService
{
}
