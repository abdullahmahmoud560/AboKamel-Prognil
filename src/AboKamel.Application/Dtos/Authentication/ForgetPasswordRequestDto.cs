using System.ComponentModel.DataAnnotations;

namespace Services.Application.Dtos.Authentication;

public class ForgetPasswordRequestDto
{
    [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    public string Email { get; set; } = string.Empty;
}