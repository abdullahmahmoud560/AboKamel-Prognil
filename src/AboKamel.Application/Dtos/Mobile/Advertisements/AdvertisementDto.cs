namespace AboKamel.Application.Dtos.Mobile.Advertisements;

public class AdvertisementDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> ImageUrls { get; set; } = new();
}