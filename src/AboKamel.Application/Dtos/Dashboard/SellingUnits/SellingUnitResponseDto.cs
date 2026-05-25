using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.SellingUnits;

public class SellingUnitResponseDto : BaseResponseDto<int>
{
    public string Name { get; set; } = string.Empty;
}