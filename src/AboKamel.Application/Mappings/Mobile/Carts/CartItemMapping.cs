using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Domain.Entities.Carts;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Mobile.Carts;

public static class CartItemMapping
{
    public static void AddCartItemMapping(this MappingProfiles map)
    {
        map.CreateMap<CartItemRequestDto, CartItem>();
        map.CreateMap<CartItem, CartItemResponseDto>();
        map.CreateMap<CartItem, CartItemDetailedResponseDto>();
    }
}
