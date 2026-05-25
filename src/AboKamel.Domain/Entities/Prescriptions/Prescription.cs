using Capsula.Domain.Entities.Carts;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Prescriptions;

public class Prescription : AuditableEntity<int>
{
    public int CartId { get; set; }
    public Cart Cart { get; set; }

    public string Description { get; set; } = string.Empty;

    public ICollection<VoiceRecord> VoiceRecords { get; set; } = new List<VoiceRecord>();
    public ICollection<PrescriptionImage> PrescriptionImages { get; set; } = new List<PrescriptionImage>();
}
