using AboKamel.Core.Dtos.Analysis;
using Services.Core.DependencyInjection;

namespace AboKamel.Application.Contracts.Dashboard.DashboardAnalysis;

public interface IAnalysisService : IApplicationService, IScopedService
{
    // =========================
    // Revenue
    // =========================
    Task<decimal> GetRevenueForPeriodAsync(DateOnly from, DateOnly to);
    Task<List<DailyRevenueDto>> GetDailyRevenueAsync(DateOnly from, DateOnly to);

    // =========================
    // Orders analysis
    // =========================
    Task<OrderStatusPercentageDto> GetDeliveredAndCancelledPercentageAsync();

    // =========================
    // Sales totals
    // =========================
    Task<SalesPeriodDto> GetWeeklyAndMonthlySalesAsync();

    Task<List<TopSellingCategoryDto>> GetTopSellingCategoriesAsync(int top = 5);

    Task<List<OrderStatusCountDto>> GetOrderCountByStatusAsync();

    Task<int> GetProductsCount();
}