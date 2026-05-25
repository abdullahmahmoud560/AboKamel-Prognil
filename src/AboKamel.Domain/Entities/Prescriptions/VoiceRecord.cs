using Services.Core.Entities;

namespace Capsula.Domain.Entities.Prescriptions;

public class VoiceRecord : BaseEntity<int>
{
    public int PrescriptionId { get; set; }
    public Prescription Prescription { get; set; }

    public string VoicePath { get; set; } = string.Empty;
}
