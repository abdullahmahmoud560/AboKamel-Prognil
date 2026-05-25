using AboKamel.Core.Enums;
using Capsula.Application.Dtos;
using Services.Core.Dtos;
using Services.Domain.Entities.Users;
using System.Text.Json.Serialization;

namespace AboKamel.Application.Dtos.Dashboard.Debts;

public class DebtRequestDto : BaseRequestDto
{
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DebitCredit DebitCredit { get; set; }
    [JsonIgnore]
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
}