using Microsoft.AspNetCore.Http;

namespace Capsula.Application.Dtos.Dashboard.Brands;

public class BrandRequestDto : BaseRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public IFormFile? ImageFile { get; set; }
}
