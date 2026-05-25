using AboKamel.Application.Dtos.Dashboard.Advertisements;
using AboKamel.Application.Dtos.Mobile.Advertisements;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace AboKamel.Application.Contracts.Dashboard.Advertisements;

public interface IAdvertisementService : IApplicationService, IScopedService
{
    Task<ResultAbstract<int>> CreateAsync(CreateAdvertisementRequest request);
    Task UpdateAsync(int id, UpdateAdvertisementRequest request);

    Task DeleteAsync(int id);
    Task<ResultAbstract<List<AdvertisementDto>>> SearchByNameAsync(string name);
}