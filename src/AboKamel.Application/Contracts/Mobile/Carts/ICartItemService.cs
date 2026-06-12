using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Domain.Entities.Carts;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Carts;

public interface ICartItemService : IApplicationService, IScopedService
{
    Task<ResultAbstract<CartItemResponseDto>> AddItemToCartAsync(CartItemRequestDto request, string? customerId = null);
    Task<ResultAbstract<CartItemResponseDto>> UpdateCartItemQuantityAsync(CartItemRequestDto request, string? customerId = null);
    Task<ResultAbstract<CartItemResponseDto>> DeleteCartItemAsync(int productId, int productSellingUnitId, string? customerId = null);
}