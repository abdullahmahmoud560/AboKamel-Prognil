namespace Capsula.Application.Dtos.Mobile.Prescriptions;

public class PrescriptionImageResponserDto : BaseResponseDto<int>
{
    public string ImageUrl { get; set; } = string.Empty;
}
