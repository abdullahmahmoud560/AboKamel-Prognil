namespace Capsula.Application.Dtos.Dashboard.Products;

public class ProductSearchRequestDto
{
    public string ProductName { get; set; } = string.Empty;
    public string ProductBrand { get; set; } = string.Empty;
    public string ProductCategory { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
}
