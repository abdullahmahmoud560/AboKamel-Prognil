using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Entities.Products;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Categories;

public class Category : AuditableEntity<int>
{
    public string Name { get; set; } = string.Empty!;
    public string? Slug { get; set; }
    public string? ImagePath { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Brand> Brands { get; set; } = new List<Brand>();
}
