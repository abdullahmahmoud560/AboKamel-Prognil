using Capsula.Core.Enums;
using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Carts;

public class CartItemRequestDto : BaseRequestDto
{
    public int Quantity { get; set; }
    [JsonIgnore]
    public int CartId { get; set; }
    public int ProductSellingUnitId { get; set; }
    public int ProductId { get; set; }
}
