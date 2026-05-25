using AutoMapper;
using Azure.Core;
using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Domain.Entities.Carts;
using Capsula.Domain.Repositories.Carts;
using Microsoft.Extensions.Logging;
using Services.Core.Dtos;
using Services.Core.Results;

namespace Capsula.Application.Services.Mobile.Carts;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    private readonly ILogger<CartItem> _logger;

    public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper, ILogger<CartItem> logger, ICartService cartService)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
        _logger = logger;
        _cartService = cartService;
    }

    public async Task<ResultAbstract<CartItemResponseDto>> AddItemToCartAsync(CartItemRequestDto request, string customerId)
    {
        CartItem cartItem;
        var cart = await _cartService.GetCustomerCartAsync(customerId);

        if(cart is null)
        {
            cart = await _cartService.InitializeCustomerCartAsync(customerId);
        }

        cartItem = await _cartItemRepository.GetCustomerCartItemAsync(request.ProductId, request.ProductSellingUnitId, customerId);

        if(cartItem is not null)
        {
            var result = await UpdateCartItemQuantityAsync(request, customerId);
            return result;
        }
        else
        {
            request.CartId = cart.Id;

            cartItem = _mapper.Map<CartItem>(request);
            var isCartItemAdded = await _cartItemRepository.AddAsync(cartItem);

            if (!isCartItemAdded)
            {
                _logger.LogError($"Could not add cart item for customer: {customerId}");
                return Result.Error($"Could not add cart item for customer: {customerId}");
            }
        }

        return Result.Success(_mapper.Map<CartItemResponseDto>(cartItem));
    }

    public async Task<ResultAbstract<CartItemResponseDto>> UpdateCartItemQuantityAsync(CartItemRequestDto request, string customerId)
    {
        var cartItem = await _cartItemRepository.GetCustomerCartItemAsync(request.ProductId, request.ProductSellingUnitId, customerId);

        if(cartItem is null)
        {
            _logger.LogError($"Could not find cart item for custmer {customerId}");
            return Result.Error("Could not find cart item");
        }

        cartItem.Quantity = request.Quantity;
        var isCartItemUpdated = await _cartItemRepository.EditAsync(cartItem);

        if (!isCartItemUpdated)
        {
            _logger.LogError($"Could not update cart item quantity for customer: {customerId}");
            return Result.Error($"Could not update cart item quantity for customer: {customerId}");
        }

        return Result.Success(_mapper.Map<CartItemResponseDto>(cartItem));
    }

    public async Task<ResultAbstract<CartItemResponseDto>> DeleteCartItemAsync(int productId, int productSellingUnitId, string customerId)
    {
        var cartItem = await _cartItemRepository.GetCustomerCartItemAsync(productId, productSellingUnitId, customerId);

        if (cartItem is null)
        {
            _logger.LogError($"Could not find cart item for custmer {customerId}");
            return Result.Error("Could not find cart item");
        }

        var isCartItemdELETED = await _cartItemRepository.DeleteAsync(cartItem);

        if (!isCartItemdELETED)
        {
            _logger.LogError($"Could not delete cart item for customer: {customerId}");
            return Result.Error($"Could not delete cart item for customer: {customerId}");
        }

        return Result.Success(_mapper.Map<CartItemResponseDto>(cartItem));
    }
}
