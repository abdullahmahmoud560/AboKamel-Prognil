using Capsula.Application.Dtos;

namespace Services.Application.Dtos.Authentication;

public class BaseUserResponseDto : BaseResponseDto<string>
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
