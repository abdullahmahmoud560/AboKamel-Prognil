using Capsula.Domain.Entities.Prescriptions;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace Capsula.Domain.Repositories.Carts;

public interface IPrescriptionRepository : IRepository<Prescription, int>, IApplicationService, IScopedService
{
    public Task<Prescription> GetCartPrescription(int cartId);
}
