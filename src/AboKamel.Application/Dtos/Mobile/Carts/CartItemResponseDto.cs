using Capsula.Core.Enums;

namespace Capsula.Application.Dtos.Mobile.Carts;

public class CartItemResponseDto : BaseResponseDto<int>
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public int ProductSellingUnitId { get; set; }
}
