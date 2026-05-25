using Capsula.Domain.Entities.Products;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Orders;

public class OrderItem : AuditableEntity<int>
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal => Quantity * Price;
    public string SellingUnitName { get; set; } = string.Empty;
    public int SellingUnitId { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}