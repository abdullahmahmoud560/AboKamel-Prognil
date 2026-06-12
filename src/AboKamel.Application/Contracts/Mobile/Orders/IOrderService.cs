using AboKamel.Application.Dtos.Dashboard.Orders;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace AboKamel.Application.Contracts.Mobile.Orders;

public interface IOrderService : IApplicationService, IScopedService
{
    Task<ResultAbstract<OrderResponseDto>> CreateOrderAsync(string? customerId = null);
    Task<ResultAbstract<List<OrderResponseDto>>> GetCustomerOrdersAsync(string? customerId = null);
    Task<ResultAbstract<OrderResponseDto>> GetCustomerOrderOrderByIdAsync(int orderId, string? customerId = null);
    Task<ResultAbstract<List<ProductResponseDto>>> GetCustomerLastOrderedProductsAsync(string? customerId = null);
}