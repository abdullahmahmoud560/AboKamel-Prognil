namespace Capsula.Application.Dtos.Mobile.Prescriptions;

public class VoiceRecordResponseDto : BaseResponseDto<int>
{
    public string VoiceUrl { get; set; } = string.Empty;
}
