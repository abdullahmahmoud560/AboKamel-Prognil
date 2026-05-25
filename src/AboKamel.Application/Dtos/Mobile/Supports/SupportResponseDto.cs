using Capsula.Core.Enums;
using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Supports;

public class SupportResponseDto : BaseResponseDto<int>
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SupportStatus Status { get; set; }
}
