using Capsula.Application.Dtos.Mobile.Products;
using Capsula.Domain.Entities.Products;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Mobile.Products;

public static class FavoriteMapping
{
    public static void AddFavoriteMapping(this MappingProfiles map)
    {
        map.CreateMap<FavoriteRequestDto, Favorite>();
        map.CreateMap<FavoriteRequestDto, Favorite>();
        map.CreateMap<Favorite, FavoriteResponseDto>();
        map.CreateMap<Product, ProductFavoriteDetailedResponseDto>();
    }
}
