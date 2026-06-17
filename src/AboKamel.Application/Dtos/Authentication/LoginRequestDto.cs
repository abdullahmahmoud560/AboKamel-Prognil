using System.ComponentModel.DataAnnotations;

namespace Services.Application.Dtos.Authentication;

public class LoginRequestDto
{
    [Required(ErrorMessage = "المعرف مطلوب")]
    public string Identifier { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    public string Password { get; set; } = string.Empty;
}
