namespace Capsula.Application.Dtos.Mobile.Prescriptions;

public class PrescriptionDetailedResponseDto : BaseResponseDto<int>
{
    public string Description { get; set; } = string.Empty;

    public ICollection<PrescriptionImageResponserDto> PrescriptionImages { get; set; }
    public ICollection<VoiceRecordResponseDto> VoiceRecords { get; set; }
}