using AboKamel.Core.Enums;
using Capsula.Domain.Entities.Users.Customers;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Orders;

public class Order : AuditableEntity<int>
{
    public string CustomerId { get; set; } = string.Empty;
    public Customer Customer { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;
    public string DetailedAddress { get; set; } = string.Empty;
    public decimal? Discount { get; set; }

    public DateOnly? ArrivalDate { get; set; }
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
