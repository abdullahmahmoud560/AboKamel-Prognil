using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Products;

public class FavoriteRequestDto : BaseRequestDto
{
    public int ProductId { get; set; }

    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
}
