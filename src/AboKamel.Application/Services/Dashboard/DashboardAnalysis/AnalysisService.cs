using AboKamel.Application.Contracts.Dashboard.DashboardAnalysis;
using AboKamel.Core.Dtos.Analysis;
using AboKamel.Domain.Repositories.Orders;

namespace AboKamel.Application.Services.Dashboard.DashboardAnalysis;

public class AnalysisService : IAnalysisService
{
    private readonly IOrderRepository _orderRepository;

    public AnalysisService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    // =========================
    // Revenue
    // =========================

    public async Task<decimal> GetRevenueForPeriodAsync(DateOnly from, DateOnly to)
    {
        if (from > to)
            throw new ArgumentException("From date must be earlier than To date");

        return await _orderRepository.GetRevenueForPeriodAsync(from, to);
    }

    public async Task<List<DailyRevenueDto>> GetDailyRevenueAsync(DateOnly from, DateOnly to)
    {
        if (from > to)
            throw new ArgumentException("From date must be earlier than To date");

        return await _orderRepository.GetDailyRevenueAsync(from, to);
    }

    // =========================
    // Orders percentage
    // =========================

    public async Task<OrderStatusPercentageDto> GetDeliveredAndCancelledPercentageAsync()
    {
        return await _orderRepository.GetDeliveredAndCancelledPercentageAsync();
    }

    // =========================
    // Sales totals
    // =========================

    public async Task<SalesPeriodDto> GetWeeklyAndMonthlySalesAsync()
    {
        return await _orderRepository.GetWeeklyAndMonthlySalesAsync();
    }

    public async Task<List<TopSellingCategoryDto>> GetTopSellingCategoriesAsync(int top = 5)
    {
        if (top <= 0)
            throw new ArgumentException("Top value must be greater than zero");

        return await _orderRepository.GetTopSellingCategoriesAsync(top);
    }

    public async Task<List<OrderStatusCountDto>> GetOrderCountByStatusAsync()
    {
        return await _orderRepository.GetOrderCountByStatusAsync();
    }

    public async Task<int> GetProductsCount()
    {
        return await _orderRepository.GetProductsCount();
    }
}