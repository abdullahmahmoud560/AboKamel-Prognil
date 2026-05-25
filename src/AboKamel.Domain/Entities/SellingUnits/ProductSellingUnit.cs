using Capsula.Domain.Entities.Products;
using Services.Core.Entities;

namespace AboKamel.Domain.Entities.SellingUnits;

public class ProductSellingUnit : BaseEntity<int>
{
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public int SellingUnitId { get; set; }
    public virtual SellingUnit SellingUnit { get; set; }
}