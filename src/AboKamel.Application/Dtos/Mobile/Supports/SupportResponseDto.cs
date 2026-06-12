using Capsula.Core.Enums;
using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Supports;

public class SupportResponseDto : BaseResponseDto<int>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? AttachmentPath { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SupportStatus Status { get; set; }
}