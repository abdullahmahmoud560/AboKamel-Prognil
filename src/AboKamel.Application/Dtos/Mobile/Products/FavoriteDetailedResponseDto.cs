using Capsula.Application.Dtos.Dashboard.Products;

namespace Capsula.Application.Dtos.Mobile.Products;

public class FavoriteDetailedResponseDto : BaseResponseDto<int>
{
    public int ProductId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ProductResponseDto Product { get; set; }
}
