
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Products;

public class Favorite : AuditableEntity<int>
{
    public int ProductId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public virtual Product Product { get; set; }
}