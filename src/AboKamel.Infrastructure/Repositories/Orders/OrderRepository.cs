using AboKamel.Core.Dtos.Analysis;
using AboKamel.Core.Enums;
using AboKamel.Domain.Repositories.Orders;
using Capsula.Domain.Entities.Orders;
using Capsula.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace AboKamel.Infrastructure.Repositories.Orders;

public class OrderRepository : Repository<Order, int>, IOrderRepository
{
    private readonly CapsulaDbContext _context;

    public OrderRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Order> GetCustomerOrderByIdAsync(string customerId, int orderId)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
            .Where(o => o.CustomerId == customerId && o.Id == orderId).SingleOrDefaultAsync();
    }

    public async Task<List<Order>> GetCustomerOrdersAsync(string customerId)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
            .Where(o => o.CustomerId == customerId).ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
            .Where(o => o.Id == orderId).SingleOrDefaultAsync();
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
            .Where(o => o.Status == status).ToListAsync();
    }

    // =========================
    // Revenue for period
    // =========================

    public async Task<decimal> GetRevenueForPeriodAsync(DateOnly from, DateOnly to)
    {
        return await _context.OrderItems
            .Where(i =>
                i.Order.CreatedDate >= from.ToDateTime(TimeOnly.MinValue) &&
                i.Order.CreatedDate <= to.ToDateTime(TimeOnly.MaxValue))
            .SumAsync(i => i.Quantity * i.Price);
    }

    public async Task<List<DailyRevenueDto>> GetDailyRevenueAsync(DateOnly from, DateOnly to)
    {
        return await _context.OrderItems
            .Where(i =>
                i.Order.CreatedDate >= from.ToDateTime(TimeOnly.MinValue) &&
                i.Order.CreatedDate <= to.ToDateTime(TimeOnly.MaxValue))
            .GroupBy(i => DateOnly.FromDateTime(i.Order.CreatedDate!.Value))
            .Select(g => new DailyRevenueDto
            {
                Date = g.Key,
                Revenue = g.Sum(x => x.Quantity * x.Price)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    // =========================
    // Delivered & cancelled %
    // =========================

    public async Task<OrderStatusPercentageDto> GetDeliveredAndCancelledPercentageAsync()
    {
        var totalOrders = await _context.Orders.CountAsync();

        if (totalOrders == 0)
            return new OrderStatusPercentageDto();

        var delivered = await _context.Orders
            .CountAsync(o => o.Status == OrderStatus.Delivered);

        var cancelled = await _context.Orders
            .CountAsync(o => o.Status == OrderStatus.Cancelled);

        return new OrderStatusPercentageDto
        {
            DeliveredPercentage = Math.Round((decimal)delivered / totalOrders * 100, 2),
            CancelledPercentage = Math.Round((decimal)cancelled / totalOrders * 100, 2)
        };
    }

    // =========================
    // Weekly & Monthly sales
    // =========================

    public async Task<SalesPeriodDto> GetWeeklyAndMonthlySalesAsync()
    {
        var today = DateTime.UtcNow;

        var weekStart = today.Date.AddDays(-(int)today.DayOfWeek);
        var monthStart = new DateTime(today.Year, today.Month, 1);

        var weeklySales = await _context.OrderItems
            .Where(i => i.Order.CreatedDate >= weekStart)
            .SumAsync(i => i.Quantity * i.Price);

        var monthlySales = await _context.OrderItems
            .Where(i => i.Order.CreatedDate >= monthStart)
            .SumAsync(i => i.Quantity * i.Price);

        return new SalesPeriodDto
        {
            WeeklySales = weeklySales,
            MonthlySales = monthlySales
        };
    }

    public async Task<List<TopSellingCategoryDto>> GetTopSellingCategoriesAsync(int top = 5)
    {
        return await _context.OrderItems
            .Include(i => i.Product)
                .ThenInclude(p => p.Category)
            .GroupBy(i => new
            {
                i.Product.CategoryId,
                i.Product.Category.Name
            })
            .Select(g => new TopSellingCategoryDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Quantity * x.Price)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .Take(top)
            .ToListAsync();
    }

    public async Task<List<OrderStatusCountDto>> GetOrderCountByStatusAsync()
    {
        return await _context.Orders
            .GroupBy(o => o.Status)
            .Select(g => new OrderStatusCountDto
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
    }

    public async Task<int> GetProductsCount()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<OrderItem> GetOrderItemByOrderIdAndProductId(int orderId, int productId)
    {
        return await _context.OrderItems.Where(i => i.OrderId == orderId && i.ProductId == productId).SingleOrDefaultAsync();
    }


    public async Task<List<Product>> GetCustomerLastOrderedProductsAsync(string customerId, int orderCount = 1)
    {
        // Get the last 'orderCount' orders for the customer
        var lastOrders = await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.ArrivalDate)
            .Take(orderCount)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        // Select the products from all order items
        var products = lastOrders
            .SelectMany(o => o.Items)
            .Select(oi => oi.Product)
            .Distinct() // optional: remove duplicates if a product appears multiple times
            .ToList();

        return products;
    }

    // =========================
    // Low stock products
    // =========================

    //public async Task<List<LowStockProductDto>> GetLowStockProductsAsync(int threshold)
    //{
    //    return await _context.Products
    //        .Where(p => p.ProductSellingUnits <= threshold)
    //        .Select(p => new LowStockProductDto
    //        {
    //            ProductId = p.Id,
    //            ProductName = p.Name,
    //            CurrentStock = p.StockQuantity
    //        })
    //        .OrderBy(p => p.CurrentStock)
    //        .ToListAsync();
    //}
}
