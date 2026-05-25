using AboKamel.Core.Enums;
using Capsula.Domain.Entities.Users.Customers;
using Services.Core.Entities;

namespace AboKamel.Domain.Entities.Debts;

public class Debt : AuditableEntity<int>
{
    public string CustomerId { get; set; } = string.Empty;
    public virtual Customer Customer { get; set; }
    public decimal Amount { get; set; }
    public DebitCredit DebitCredit { get; set; }
}