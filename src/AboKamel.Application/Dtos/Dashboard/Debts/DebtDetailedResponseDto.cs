using AboKamel.Core.Enums;
using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.Debts;

public class DebtDetailedResponseDto : BaseResponseDto<int>
{
    public string CustomerId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DebitCredit DebitCredit { get; set; }
    public DateTime? CreatedDate { get; set; }
}
