namespace Capsula.Application.Dtos.Dashboard.Categories;

public class CategoryResponseDto : BaseAuditableResponseDto<int>
{
    public string Name { get; set; } = string.Empty!;
    public string? Slug { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
}
