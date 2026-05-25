using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using AboKamel.Domain.Entities.SellingUnits;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Core.Enums;

namespace Capsula.Application.Dtos.Mobile.Carts;

public class CartItemDetailedResponseDto : BaseResponseDto<int>
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public int ProductSellingUnitId { get; set; }
    public ProductSellingUnitResponseDto ProductSellingUnit { get; set; }
    public ProductDetailedResponseDto Product { get; set; }
}
