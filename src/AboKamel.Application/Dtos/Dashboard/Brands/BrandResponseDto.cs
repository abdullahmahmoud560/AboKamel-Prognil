namespace Capsula.Application.Dtos.Dashboard.Brands;

public class BrandResponseDto : BaseAuditableResponseDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}
