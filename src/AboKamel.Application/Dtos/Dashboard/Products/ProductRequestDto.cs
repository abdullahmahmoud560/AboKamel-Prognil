using Microsoft.AspNetCore.Http;

namespace Capsula.Application.Dtos.Dashboard.Products;

public class ProductRequestDto : BaseRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }
    public int MinimumQuantityPerOrder { get; set; }
    public int MaximumQuantityPerOrder { get; set; }
    public string? Status { get; set; }

    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public int AreaId { get; set; }
}
