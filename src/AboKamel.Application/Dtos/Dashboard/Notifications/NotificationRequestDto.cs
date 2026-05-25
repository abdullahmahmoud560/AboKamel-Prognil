namespace AboKamel.Application.Dtos.Dashboard.Notifications;

public class NotificationRequestDto
{
    public string? CustomerId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AdditionalText { get; set; } = string.Empty;
    public int? AreaId { get; set; }
}