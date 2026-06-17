using AutoMapper;
using Azure.Core;
using AboKamel.Domain.Entities.SellingUnits;
using AboKamel.Domain.Repositories.SellingUnits;
using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Domain.Entities.Carts;
using Capsula.Domain.Repositories.Carts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core.Dtos;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Application.Services.Mobile.Carts;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    private readonly ILogger<CartItem> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductSellingUnitRepository _productSellingUnitRepository;

    public CartItemService(
        ICartItemRepository cartItemRepository, 
        IMapper mapper, 
        ILogger<CartItem> logger, 
        ICartService cartService, 
        IHttpContextAccessor httpContextAccessor,
        IProductSellingUnitRepository productSellingUnitRepository)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
        _logger = logger;
        _cartService = cartService;
        _httpContextAccessor = httpContextAccessor;
        _productSellingUnitRepository = productSellingUnitRepository;
    }

    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }
        return userId;
    }

    public async Task<ResultAbstract<CartItemResponseDto>> AddItemToCartAsync(CartItemRequestDto request, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();

        // First check inventory
        var sellingUnit = await _productSellingUnitRepository.GetByIdAsync(request.ProductSellingUnitId);
        if (sellingUnit == null)
        {
            _logger.LogError($"Could not find product selling unit with id: {request.ProductSellingUnitId}");
            return Result.Error("لم يتم العثور على المنتج");
        }

        // Check if requested quantity is available in stock
        if (request.Quantity > sellingUnit.Quantity)
        {
            return Result.Error($"الكمية المطلوبة غير متوفرة. الحد الأقصى المتوفر هو {sellingUnit.Quantity}");
        }

        CartItem cartItem;
        var cart = await _cartService.GetCustomerCartAsync(currentUserId);

        if(cart is null)
        {
            cart = await _cartService.InitializeCustomerCartAsync(currentUserId);
        }

        cartItem = await _cartItemRepository.GetCustomerCartItemAsync(request.ProductId, request.ProductSellingUnitId, currentUserId);

        if(cartItem is not null)
        {
            // If item exists, calculate total quantity and check inventory
            var totalQuantity = cartItem.Quantity + request.Quantity;
            if (totalQuantity > sellingUnit.Quantity)
            {
                return Result.Error($"الكمية المطلوبة غير متوفرة. الحد الأقصى المتوفر هو {sellingUnit.Quantity}");
            }
            
            // Update the request with the total quantity and call update method
            request.Quantity = totalQuantity;
            var result = await UpdateCartItemQuantityAsync(request, currentUserId);
            return result;
        }
        else
        {
            request.CartId = cart.Id;

            cartItem = _mapper.Map<CartItem>(request);
            var isCartItemAdded = await _cartItemRepository.AddAsync(cartItem);

            if (!isCartItemAdded)
            {
                _logger.LogError($"Could not add cart item for customer: {currentUserId}");
                return Result.Error($"لم يمكن إضافة العنصر إلى السلة");
            }
        }

        _logger.LogInformation("Added cart item for customer {UserId}, product {ProductId}", currentUserId, request.ProductId);
        return Result.Success(_mapper.Map<CartItemResponseDto>(cartItem));
    }

    public async Task<ResultAbstract<CartItemResponseDto>> UpdateCartItemQuantityAsync(CartItemRequestDto request, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var cartItem = await _cartItemRepository.GetCustomerCartItemAsync(request.ProductId, request.ProductSellingUnitId, currentUserId);

        if(cartItem is null)
        {
            _logger.LogError($"Could not find cart item for customer {currentUserId}");
            return Result.Error("لم يتم العثور على عنصر السلة");
        }

        // Check inventory for new quantity
        var sellingUnit = await _productSellingUnitRepository.GetByIdAsync(request.ProductSellingUnitId);
        if (sellingUnit == null)
        {
            _logger.LogError($"Could not find product selling unit with id: {request.ProductSellingUnitId}");
            return Result.Error("لم يتم العثور على المنتج");
        }

        if (request.Quantity > sellingUnit.Quantity)
        {
            return Result.Error($"الكمية المطلوبة غير متوفرة. الحد الأقصى المتوفر هو {sellingUnit.Quantity}");
        }

        if (request.Quantity <= 0)
        {
            return Result.Error("الكمية يجب أن تكون أكبر من صفر");
        }

        cartItem.Quantity = request.Quantity;
        var isCartItemUpdated = await _cartItemRepository.EditAsync(cartItem);

        if (!isCartItemUpdated)
        {
            _logger.LogError($"Could not update cart item quantity for customer: {currentUserId}");
            return Result.Error($"لم يمكن تحديث كمية عنصر السلة");
        }

        _logger.LogInformation("Updated cart item quantity for customer {UserId}, product {ProductId}", currentUserId, request.ProductId);
        return Result.Success(_mapper.Map<CartItemResponseDto>(cartItem));
    }

    public async Task<ResultAbstract<CartItemResponseDto>> DeleteCartItemAsync(int productId, int productSellingUnitId, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var cartItem = await _cartItemRepository.GetCustomerCartItemAsync(productId, productSellingUnitId, currentUserId);

        if (cartItem is null)
        {
            _logger.LogError($"Could not find cart item for custmer {currentUserId}");
            return Result.Error("Could not find cart item");
        }

        var isCartItemdELETED = await _cartItemRepository.DeleteAsync(cartItem);

        if (!isCartItemdELETED)
        {
            _logger.LogError($"Could not delete cart item for customer: {currentUserId}");
            return Result.Error($"Could not delete cart item for customer: {currentUserId}");
        }

        return Result.Success(_mapper.Map<CartItemResponseDto>(cartItem));
    }
}
