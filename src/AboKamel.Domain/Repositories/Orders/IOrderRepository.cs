using AboKamel.Core.Dtos.Analysis;
using AboKamel.Core.Enums;
using Capsula.Domain.Entities.Orders;
using Capsula.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace AboKamel.Domain.Repositories.Orders;

public interface IOrderRepository : IRepository<Order, int>, IApplicationService, IScopedService
{
    Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<OrderItem> GetOrderItemByOrderIdAndProductId(int orderId, int productId);
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<List<Order>> GetOrdersAsync();
    Task<List<Order>> GetCustomerOrdersAsync(string customerId);
    Task<Order> GetCustomerOrderByIdAsync(string customerId, int orderId);

    // =========================
    // Revenue reports
    // =========================
    Task<decimal> GetRevenueForPeriodAsync(DateOnly from, DateOnly to);
    Task<List<DailyRevenueDto>> GetDailyRevenueAsync(DateOnly from, DateOnly to);

    // =========================
    // Orders percentage
    // =========================
    Task<OrderStatusPercentageDto> GetDeliveredAndCancelledPercentageAsync();

    // =========================
    // Sales totals
    // =========================
    Task<SalesPeriodDto> GetWeeklyAndMonthlySalesAsync();

    // =========================
    // Products
    // =========================
    //Task<List<LowStockProductDto>> GetLowStockProductsAsync(int threshold);

    // =========================
    // Categories analysis
    // =========================
    Task<List<TopSellingCategoryDto>> GetTopSellingCategoriesAsync(int top = 5);

    Task<List<OrderStatusCountDto>> GetOrderCountByStatusAsync();
    Task<int> GetProductsCount();

    Task<List<Product>> GetCustomerLastOrderedProductsAsync(string customerId, int orderCount = 1);
}
