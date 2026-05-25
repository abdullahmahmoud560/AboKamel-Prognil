using Services.Core.Entities;

namespace AboKamel.Domain.Entities.SellingUnits;


public class SellingUnit : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<ProductSellingUnit> ProductSellingUnits { get; set; } = new List<ProductSellingUnit>();
}