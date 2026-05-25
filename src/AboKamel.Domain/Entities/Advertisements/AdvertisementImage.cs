using Services.Core.Entities;

namespace AboKamel.Domain.Entities.Advertisements;

public class AdvertisementImage : BaseEntity<int>
{
    public string Url { get; set; } = string.Empty;

    public int AdvertisementId { get; set; }
    public Advertisement Advertisement { get; set; } = null!;
}
