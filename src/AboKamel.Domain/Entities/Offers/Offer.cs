using AboKamel.Domain.Entities.Areas;
using Capsula.Domain.Entities.Products;
using Services.Core.Entities;

namespace AboKamel.Domain.Entities.Offers;

public class Offer : AuditableEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public virtual Product Product { get; set; }
    public int ProductId { get; set; }

    public virtual Area Area { get; set; }
    public int AreaId { get; set; }
}