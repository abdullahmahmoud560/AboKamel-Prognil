namespace Services.Application.Dtos.Authentication;

public class LoginResponseDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public required List<string> Roles { get; set; }
    public string Token { get; set; } = string.Empty;
}
