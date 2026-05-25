using AboKamel.Application.Dtos.Dashboard.SellingUnits;

namespace Capsula.Application.Dtos.Dashboard.Products;

public class ProductResponseDto : BaseAuditableResponseDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public int MinimumQuantityPerOrder { get; set; }
    public int MaximumQuantityPerOrder { get; set; }
    public string? Status { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public int AreaId { get; set; }

    public ICollection<ProductSellingUnitResponseDto> ProductSellingUnits { get; set; } = new List<ProductSellingUnitResponseDto>();
}
