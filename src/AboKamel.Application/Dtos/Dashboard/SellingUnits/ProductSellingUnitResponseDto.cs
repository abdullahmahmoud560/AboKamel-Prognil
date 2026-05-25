using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.SellingUnits;

public class ProductSellingUnitResponseDto : BaseResponseDto<int>
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int SellingUnitId { get; set; }
    public string SellingUnitName { get; set; } = string.Empty;
    public SellingUnitResponseDto SellingUnit { get; set; } = default;
}
