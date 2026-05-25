using Capsula.Application.Dtos;
using System.Text.Json.Serialization;

namespace AboKamel.Application.Dtos.Dashboard.SellingUnits;

public class ProductSellingUnitRequestDto : BaseRequestDto
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int SellingUnitId { get; set; }
}
