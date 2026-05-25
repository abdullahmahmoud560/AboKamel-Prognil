using Capsula.Core.Enums;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Supports;

public class Support : AuditableEntity<int>
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SupportStatus Status { get; set; }
}
