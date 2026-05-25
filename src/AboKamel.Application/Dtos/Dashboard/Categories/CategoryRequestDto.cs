using Microsoft.AspNetCore.Http;

namespace Capsula.Application.Dtos.Dashboard.Categories;

public class CategoryRequestDto : BaseRequestDto
{
    public string Name { get; set; } = string.Empty!;
    public string? Slug { get; set; }
    public IFormFile? ImageFile { get; set; }
}
