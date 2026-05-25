using Capsula.Domain.Entities.Products;
using Services.Core.Entities;

namespace AboKamel.Domain.Entities.Areas;

public class Area : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public int? MinimumQuantityPerOrder { get; set; }
    public int? MaximumQuantityPerOrder { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}