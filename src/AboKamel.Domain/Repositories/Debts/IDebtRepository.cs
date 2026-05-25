using AboKamel.Domain.Entities.Debts;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace AboKamel.Domain.Repositories.Debts;

public interface IDebtRepository : IRepository<Debt, int>, IApplicationService, IScopedService
{
    Task<Debt> GetDebtDetailAsync(int id);
    Task<List<Debt>> GetDebitUsersAsync();
    Task<List<Debt>> GetCreditUsersAsync();
    Task<decimal> GetUserBalanceAsync(string userId);
}