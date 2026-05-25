using Capsula.Domain.Entities.Prescriptions;
using Capsula.Domain.Entities.Users.Customers;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Carts;

public class Cart : AuditableEntity<int>
{
    public string CustomerId { get; set; } = string.Empty;
    public Customer Customer { get; set; }

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    public Prescription Prescription { get; set; }
}
