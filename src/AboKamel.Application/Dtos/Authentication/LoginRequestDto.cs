using System.ComponentModel.DataAnnotations;

namespace Services.Application.Dtos.Authentication;

public class LoginRequestDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
