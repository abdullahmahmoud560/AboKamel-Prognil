using AboKamel.Application.Dtos.Dashboard.Orders;
using AboKamel.Core.Enums;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace AboKamel.Application.Contracts.Dashboard.Orders;

public interface IOrderService : IApplicationService, IScopedService
{
    Task<ResultAbstract<List<OrderResponseDto>>> GetOrdersByStatusAsync(OrderStatus status);
    Task<ResultAbstract<List<OrderResponseDto>>> GetAllOrdersAsync();
    Task<ResultAbstract<OrderResponseDto>> GetOrderByIdAsync(int orderId);
    Task<ResultAbstract<OrderResponseDto>> AddDiscountAsync(int orderId, decimal discount);
    Task<ResultAbstract<OrderResponseDto>> UpdateOrderStatusAsync(int orderId, OrderStatus status, DateOnly? arrivalDate, string? notes);
    Task<ResultAbstract<OrderResponseDto>> UpdateOrderItemQuantityAsync(int orderId, int productId, int quantity);
}
