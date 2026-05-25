using AboKamel.Application.Dtos.Dashboard.Areas;
using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using AboKamel.Domain.Entities.SellingUnits;
using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Application.Dtos.Dashboard.Categories;

namespace Capsula.Application.Dtos.Dashboard.Products;

public class ProductDetailedResponseDto : BaseAuditableResponseDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public string? Status { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public int AreaId { get; set; }

    public BrandResponseDto Brand { get; set; } = default!;
    public CategoryResponseDto Category { get; set; } = default!;
    public AreaResponseDto Area { get; set; } = default!;
    public ICollection<ProductSellingUnitResponseDto> ProductSellingUnits { get; set; } = new List<ProductSellingUnitResponseDto>();
}
