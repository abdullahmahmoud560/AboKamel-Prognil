using Services.Core.Entities;

namespace AboKamel.Domain.Entities.Notificationns;

public class Notification : AuditableEntity<int>
{
    public string? CustomerId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AdditionalText { get; set; } = string.Empty;
    public int? AreaId { get; set; }
}