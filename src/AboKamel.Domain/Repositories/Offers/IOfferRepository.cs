using AboKamel.Domain.Entities.Offers;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace AboKamel.Domain.Repositories.Offers;

public interface IOfferRepository : IRepository<Offer, int>, IApplicationService, IScopedService
{
    public Task<List<Offer>> GetOfferWithDetailsAsync();
}
