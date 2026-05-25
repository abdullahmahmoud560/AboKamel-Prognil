using Capsula.Application.Dtos.Dashboard.Categories;
using Capsula.Application.Dtos.Dashboard.Products;

namespace Capsula.Application.Dtos.Dashboard.Brands;

public class BrandDetailedResponseDto : BaseAuditableResponseDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;

    public ICollection<ProductResponseDto> Products { get; set; } = new List<ProductResponseDto>();
    public CategoryResponseDto Category { get; set; }
    public int CategoryId { get; set; }
}
