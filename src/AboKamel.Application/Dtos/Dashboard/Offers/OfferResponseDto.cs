using AboKamel.Application.Dtos.Dashboard.Areas;
using Capsula.Application.Dtos;
using Capsula.Application.Dtos.Dashboard.Products;

namespace AboKamel.Application.Dtos.Dashboard.Offers;

public class OfferResponseDto : BaseResponseDto<int>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ProductResponseDto Product { get; set; } = new ProductResponseDto();
    public AreaResponseDto Area { get; set; } = new AreaResponseDto();
}