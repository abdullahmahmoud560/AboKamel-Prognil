using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.Areas;

public class AreaRequestDto : BaseRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int? MinimumQuantityPerOrder { get; set; }
    public int? MaximumQuantityPerOrder { get; set; }
}