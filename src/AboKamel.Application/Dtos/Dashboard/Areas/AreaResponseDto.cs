using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.Areas;

public class AreaResponseDto : BaseResponseDto<int>
{
    public string Name { get; set; } = string.Empty;
    public int? MinimumQuantityPerOrder { get; set; }
    public int? MaximumQuantityPerOrder { get; set; }
}