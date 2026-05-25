using AboKamel.Domain.Entities.Areas;
using AboKamel.Domain.Entities.Offers;
using AboKamel.Domain.Entities.SellingUnits;
using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Entities.Categories;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Products;

public class Product : AuditableEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImagePath { get; set; } = string.Empty;
    public int MinimumQuantityPerOrder { get; set; }
    public int MaximumQuantityPerOrder { get; set; }
    public string? Status { get; set; }


    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public int? AreaId { get; set; }

    public virtual Brand Brand { get; set; }
    public virtual Category Category { get; set; }
    public virtual Area? Area { get; set; }
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public virtual ICollection<ProductSellingUnit> ProductSellingUnits { get; set; } = new List<ProductSellingUnit>();
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}