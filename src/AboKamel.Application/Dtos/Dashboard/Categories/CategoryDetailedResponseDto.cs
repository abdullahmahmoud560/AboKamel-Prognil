using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Application.Dtos.Dashboard.Products;

namespace Capsula.Application.Dtos.Dashboard.Categories;

public class CategoryDetailedResponseDto : BaseAuditableResponseDto<int>
{
    public string Name { get; set; } = string.Empty!;
    public string? Slug { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;

    public ICollection<ProductResponseDto> Products { get; set; } = new List<ProductResponseDto>();
    public ICollection<BrandResponseDto> Brands { get; set; } = new List<BrandResponseDto>();
}