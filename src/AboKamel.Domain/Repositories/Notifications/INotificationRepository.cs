using AboKamel.Domain.Entities.Notificationns;
using Services.Core.DependencyInjection;
using Services.Domain.Repositories;

namespace AboKamel.Domain.Repositories.Notifications;

public interface INotificationRepository : IRepository<Notification, int>, IApplicationService, IScopedService
{
    Task<List<Notification>> GetUserNotificationsAsync(string customerId);
}
