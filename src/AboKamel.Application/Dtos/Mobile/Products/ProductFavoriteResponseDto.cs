namespace Capsula.Application.Dtos.Mobile.Products;

public class ProductFavoriteResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public decimal Price { get; set; }
    public string? Status { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}