using AboKamel.Application.Contracts.Dashboard.DashboardAnalysis;
using Capsula.Api.Controllers.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace AboKamel.Api.Controllers.Dashboard.DashboardAnalysis;

public class AnalysisController : DashboardBaseController
{
    private readonly IAnalysisService _analysisService;

    public AnalysisController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    // =========================
    // Revenue
    // =========================

    /// <summary>
    /// Total revenue for a specific period
    /// </summary>
    [HttpGet("revenue/period")]
    public async Task<IActionResult> GetRevenueForPeriod(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
    {
        var result = await _analysisService.GetRevenueForPeriodAsync(from, to);
        return Ok(result);
    }

    /// <summary>
    /// Daily revenue within a period
    /// </summary>
    [HttpGet("revenue/daily")]
    public async Task<IActionResult> GetDailyRevenue(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
    {
        var result = await _analysisService.GetDailyRevenueAsync(from, to);
        return Ok(result);
    }

    // =========================
    // Orders analysis
    // =========================

    /// <summary>
    /// Percentage of delivered vs cancelled orders
    /// </summary>
    [HttpGet("orders/status-percentage")]
    public async Task<IActionResult> GetOrdersStatusPercentage()
    {
        var result = await _analysisService.GetDeliveredAndCancelledPercentageAsync();
        return Ok(result);
    }

    // =========================
    // Sales totals
    // =========================

    /// <summary>
    /// Weekly and monthly sales totals
    /// </summary>
    [HttpGet("sales/summary")]
    public async Task<IActionResult> GetWeeklyAndMonthlySales()
    {
        var result = await _analysisService.GetWeeklyAndMonthlySalesAsync();
        return Ok(result);
    }

    // =========================
    // Categories
    // =========================

    /// <summary>
    /// Top selling categories
    /// </summary>
    [HttpGet("categories/top-selling")]
    public async Task<IActionResult> GetTopSellingCategories(
        [FromQuery] int top = 5)
    {
        var result = await _analysisService.GetTopSellingCategoriesAsync(top);
        return Ok(result);
    }

    /// <summary>
    /// Orders count grouped by status
    /// </summary>
    [HttpGet("orders/status-count")]
    public async Task<IActionResult> GetOrdersStatusCount()
    {
        var result = await _analysisService.GetOrderCountByStatusAsync();
        return Ok(result);
    }

    /// <summary>
    /// Orders count grouped by status
    /// </summary>
    [HttpGet("products/count")]
    public async Task<IActionResult> GetProductsCountAsync()
    {
        var result = await _analysisService.GetProductsCount();
        return Ok(result);
    }
}