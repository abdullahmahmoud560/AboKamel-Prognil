using System.ComponentModel.DataAnnotations;

namespace Services.Application.Dtos.Authentication;

public class ChangePasswordRequestDto
{
    [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
    public string CurrentPassword { get; set; } = "";

    [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
    public string NewPassword { get; set; } = "";

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare(nameof(NewPassword), ErrorMessage = "كلمة المرور وتأكيدها غير متطابقتين")]
    public string ConfirmPassword { get; set; } = "";
}