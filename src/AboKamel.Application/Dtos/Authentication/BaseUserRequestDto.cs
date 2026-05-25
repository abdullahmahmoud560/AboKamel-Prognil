using Capsula.Application.Dtos;

namespace Services.Application.Dtos.Authentication;

public class BaseUserRequestDto : BaseRequestDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string CustomPassword { get; set; } = string.Empty;
}
