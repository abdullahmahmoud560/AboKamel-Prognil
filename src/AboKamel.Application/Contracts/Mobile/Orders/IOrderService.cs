using AboKamel.Application.Dtos.Dashboard.Orders;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace AboKamel.Application.Contracts.Mobile.Orders;

public interface IOrderService : IApplicationService, IScopedService
{
    Task<ResultAbstract<OrderResponseDto>> CreateOrderAsync(string customerId);
    Task<ResultAbstract<List<OrderResponseDto>>> GetCustomerOrdersAsync(string customerId);
    Task<ResultAbstract<OrderResponseDto>> GetCustomerOrderOrderByIdAsync(string customerId, int orderId);
    Task<ResultAbstract<List<ProductResponseDto>>> GetCustomerLastOrderedProductsAsync(string customerId);
}