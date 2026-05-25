using Capsula.Application.Dtos;

namespace AboKamel.Application.Dtos.Dashboard.Offers;

public class OfferRequestDto : BaseRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int ProductId { get; set; }
    public int AreaId { get; set; }
}