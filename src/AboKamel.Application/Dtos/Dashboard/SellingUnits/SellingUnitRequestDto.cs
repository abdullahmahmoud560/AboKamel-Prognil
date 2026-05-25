using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.SellingUnits;

public class SellingUnitRequestDto : BaseRequestDto
{
    public string Name { get; set; } = string.Empty;
}