using AboKamel.Domain.Entities.SellingUnits;
using Capsula.Core.Enums;
using Capsula.Domain.Entities.Orders;
using Capsula.Domain.Entities.Products;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Carts;

public class CartItem : AuditableEntity<int>
{
    public int Quantity { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; }

    public int ProductSellingUnitId { get; set; }
    public ProductSellingUnit ProductSellingUnit { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}
