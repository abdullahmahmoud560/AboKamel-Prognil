namespace Services.Application.Dtos.Authentication;

public class VerifyOtpRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string OTP { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
}
