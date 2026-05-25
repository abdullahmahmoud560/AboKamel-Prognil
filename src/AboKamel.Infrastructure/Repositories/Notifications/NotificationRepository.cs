using AboKamel.Domain.Entities.Notificationns;
using AboKamel.Domain.Repositories.Notifications;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.Repositories;

namespace AboKamel.Infrastructure.Repositories.Notifications;

public class NotificationRepository : Repository<Notification, int>, INotificationRepository
{
    CapsulaDbContext _context;

    public NotificationRepository(CapsulaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);

        if (customer is null)
        {
            return await _context.Notifications.Where(n => (n.CustomerId == null || n.CustomerId == "") && (n.AreaId == null || n.AreaId == 0))
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
        }

        return await _context.Notifications.Where(n => n.CustomerId == customerId || n.CustomerId == null || n.AreaId == customer.AreaId || n.AreaId == null)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }
}