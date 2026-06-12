using AboKamel.Domain.Entities.Areas;
using Capsula.Domain.Entities.Addresses;
using Services.Domain.Entities.Users;

namespace Capsula.Domain.Entities.Users.Customers;

public class Customer : ApplicationUser
{
    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Area Area { get; set; }
    public int AreaId { get; set; }
    public bool Active { get; set; } = true;
    public string EstablishmentType { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Landmark { get; set; } = string.Empty;
}