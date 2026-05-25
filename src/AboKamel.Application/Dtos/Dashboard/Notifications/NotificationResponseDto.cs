using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.Notifications;

public class NotificationResponseDto : BaseResponseDto<int>
{
    public string? CustomerId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AdditionalText { get; set; } = string.Empty;
    public int? AreaId { get; set; }
    public DateTime CreatedDate { get; set; }
}