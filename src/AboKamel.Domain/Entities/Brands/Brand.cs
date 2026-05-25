using Capsula.Domain.Entities.Categories;
using Capsula.Domain.Entities.Products;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Brands;

public class Brand : AuditableEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; } = string.Empty;
    public string? ImagePath { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
