using System.ComponentModel.DataAnnotations;

namespace Services.Application.Dtos.Authentication;

public class ResendOtpRequestDto
{
    [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "الغرض مطلوب")]
    public string Purpose { get; set; } = string.Empty;
}
