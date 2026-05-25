using Microsoft.AspNetCore.Http;

namespace AboKamel.Application.Dtos.Dashboard.Advertisements;

public class UpdateAdvertisementRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<IFormFile>? NewImages { get; set; }
}