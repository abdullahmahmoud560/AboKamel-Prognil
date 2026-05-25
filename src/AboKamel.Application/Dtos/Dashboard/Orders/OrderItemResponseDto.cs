using Capsula.Application.Dtos;
using Capsula.Application.Dtos.Dashboard.Products;

namespace AboKamel.Application.Dtos.Dashboard.Orders;

public class OrderItemResponseDto : BaseResponseDto<int>
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal => Quantity * Price;
    public string SellingUnitName { get; set; } = string.Empty;

    public ProductResponseDto Product { get; set; } = new ProductResponseDto();
}