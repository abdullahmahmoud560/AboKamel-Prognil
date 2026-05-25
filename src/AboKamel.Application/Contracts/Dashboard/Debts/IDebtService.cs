using AboKamel.Application.Dtos.Dashboard.Debts;
using AboKamel.Domain.Entities.Debts;
using Capsula.Application.Contracts;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace AboKamel.Application.Contracts.Dashboard.Debts;

public interface IDebtService : ICrudService<DebtRequestDto, Debt, DebtResponseDto, DebtDetailedResponseDto, int>, IApplicationService, IScopedService
{
    Task<ResultAbstract<DebtResponseDto>> GetDebitDetailsAsync(int id);
    Task<ResultAbstract<List<DebtResponseDto>>> GetDebitUsersAsync();
    Task<ResultAbstract<List<DebtResponseDto>>> GetCreditUsersAsync();
    Task<ResultAbstract<decimal>> GetUserBalanceAsync(string userId);
}