using Capsula.Domain.Entities.Supports;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Supports;

public interface ISupportRepository : IRepository<Support, int>, IApplicationService, IScopedService
{

}
