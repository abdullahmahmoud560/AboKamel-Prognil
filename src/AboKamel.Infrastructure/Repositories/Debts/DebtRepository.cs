using AboKamel.Core.Enums;
using AboKamel.Domain.Entities.Debts;
using AboKamel.Domain.Repositories.Debts;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace AboKamel.Infrastructure.Repositories.Debts;

public class DebtRepository : Repository<Debt, int>, IDebtRepository
{
    CapsulaDbContext _context;
    public DebtRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Debt>> GetCreditUsersAsync()
    {
        return await _context.Debts
            .Include(d => d.Customer)
                .ThenInclude(c => c.Addresses)
            .Where(d => d.DebitCredit == DebitCredit.Credit)
            .ToListAsync();
    }

    public async Task<List<Debt>> GetDebitUsersAsync()
    {
        return await _context.Debts
            .Include(d => d.Customer)
                .ThenInclude(c => c.Addresses)
            .Where(d => d.DebitCredit == DebitCredit.Debit)
            .ToListAsync();
    }

    public async Task<Debt> GetDebtDetailAsync(int id)
    {
        return await _context.Debts
            .Include(d => d.Customer)
            .SingleOrDefaultAsync(d => d.Id == id);
    }

    public async Task<decimal> GetUserBalanceAsync(string userId)
    {
        return await _context.Debts
            .Where(d => d.CustomerId == userId)
            .SumAsync(d =>
                d.DebitCredit == DebitCredit.Credit
                    ? d.Amount
                    : -d.Amount);
    }
}
