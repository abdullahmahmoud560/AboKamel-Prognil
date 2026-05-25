using Services.Core.Entities;

namespace AboKamel.Domain.Entities.Advertisements;

public class Advertisement : BaseEntity<int>
{

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<AdvertisementImage> Images { get; set; } = new List<AdvertisementImage>();
}