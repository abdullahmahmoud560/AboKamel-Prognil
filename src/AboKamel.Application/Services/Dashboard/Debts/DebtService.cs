using AboKamel.Application.Contracts.Dashboard.Debts;
using AboKamel.Application.Dtos.Dashboard.Debts;
using AboKamel.Domain.Entities.Debts;
using AboKamel.Domain.Repositories.Debts;
using AutoMapper;
using Capsula.Application.Services;
using Microsoft.Extensions.Logging;
using Services.Core.Results;
using Services.Domain.Repositories;

namespace AboKamel.Application.Services.Dashboard.Debts;

public class DebtService : CrudService<DebtRequestDto, Debt, DebtResponseDto, DebtDetailedResponseDto, int>, IDebtService
{
    private readonly IDebtRepository _debtRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Debt> _logger;

    public DebtService(IDebtRepository repository, IMapper mapper, ILogger<Debt> logger) : base(repository, mapper, logger)
    {
        _debtRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultAbstract<DebtResponseDto>> GetDebitDetailsAsync(int id)
    {
        var debt = await _debtRepository.GetDebtDetailAsync(id);
        return Result.Success(_mapper.Map<DebtResponseDto>(debt));
    }

    public async Task<ResultAbstract<List<DebtResponseDto>>> GetCreditUsersAsync()
    {
        var creditUsers = await _debtRepository.GetCreditUsersAsync();
        return Result.Success(_mapper.Map<List<DebtResponseDto>>(creditUsers));
    }

    public async Task<ResultAbstract<List<DebtResponseDto>>> GetDebitUsersAsync()
    {
        var debitUsers = await _debtRepository.GetDebitUsersAsync();
        return Result.Success(_mapper.Map<List<DebtResponseDto>>(debitUsers));
    }

    public async Task<ResultAbstract<decimal>> GetUserBalanceAsync(string userId)
    {
        var balance = await _debtRepository.GetUserBalanceAsync(userId);
        return Result.Success(balance);
    }
}
