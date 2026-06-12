using AboKamel.Application.Contracts.Mobile.Orders;
using AboKamel.Application.Dtos.Dashboard.Orders;
using AboKamel.Core.Enums;
using AboKamel.Domain.Repositories.Orders;
using AboKamel.Domain.Repositories.SellingUnits;
using AboKamel.Infrastructure.Repositories.Orders;
using AutoMapper;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Orders;
using Capsula.Domain.Entities.Products;
using Capsula.Domain.Repositories.Addresses;
using Capsula.Domain.Repositories.Carts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core.Results;
using System.Security.Claims;

namespace AboKamel.Application.Services.Mobile.Orders;

public class OrderService : IOrderService
{
	private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IProductSellingUnitRepository _productSellingUnitRepository;
    private readonly ISellingUnitRepository _sellingUnitRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Order> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IAddressRepository addressRepository, IMapper mapper, ISellingUnitRepository sellingUnitRepository, IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
        _mapper = mapper;
        _sellingUnitRepository = sellingUnitRepository;
        _httpContextAccessor = httpContextAccessor;
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

    public async Task<ResultAbstract<OrderResponseDto>> CreateOrderAsync(string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var cart = await _cartRepository.GetCustomerCartDetailsAsync(currentUserId);

        if(cart is null || !cart.Items.Any())
        {
            return Result.Error("Your cart is empty.");
        }

        var order = _mapper.Map<Order>(cart);

        var address = await _addressRepository.GetPrimaryAddressAsync(cart.CustomerId);

        if (address is null)
        {
            return Result.Error("No primary address found for this customer.");
        }

        foreach (var item in order.Items)
        {
            var sellingUnit = await _sellingUnitRepository.GetByIdAsync(item.SellingUnitId);
            item.SellingUnitName = sellingUnit.Name;
        }

        order.DetailedAddress = address.DetailedAddress;
        order.PhoneNumber = address.PhoneNumber;
        order.Status = OrderStatus.Pending;

        await _orderRepository.AddAsync(order);

        var orderDto = _mapper.Map<OrderResponseDto>(order);

        await _cartRepository.DeleteAsync(cart);

        return Result.Success(orderDto);
    }

    public async Task<ResultAbstract<List<OrderResponseDto>>> GetCustomerOrdersAsync(string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var orders = await _orderRepository.GetCustomerOrdersAsync(currentUserId);

        return Result.Success(_mapper.Map<List<OrderResponseDto>>(orders));
    }

    public async Task<ResultAbstract<OrderResponseDto>> GetCustomerOrderOrderByIdAsync(int orderId, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var order = await _orderRepository.GetCustomerOrderByIdAsync(currentUserId, orderId);

        return Result.Success(_mapper.Map<OrderResponseDto>(order));
    }

    public async Task<ResultAbstract<List<ProductResponseDto>>> GetCustomerLastOrderedProductsAsync(string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var lastProducts = await _orderRepository.GetCustomerLastOrderedProductsAsync(currentUserId, 5);
        return Result.Success(_mapper.Map<List<ProductResponseDto>>(lastProducts));
    }
}
