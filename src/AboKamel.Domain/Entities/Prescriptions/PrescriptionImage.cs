using Services.Core.Entities;

namespace Capsula.Domain.Entities.Prescriptions;

public class PrescriptionImage : BaseEntity<int>
{
    public int PrescriptionId { get; set; }
    public Prescription Prescription { get; set; }

    public string ImagePath { get; set; } = string.Empty;
}
