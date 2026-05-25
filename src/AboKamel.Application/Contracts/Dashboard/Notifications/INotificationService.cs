using AboKamel.Application.Dtos.Dashboard.Notifications;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace AboKamel.Application.Contracts.Dashboard.Notifications;

public interface INotificationService : IApplicationService, IScopedService
{
    Task<ResultAbstract<bool>> SendNotificationASync(NotificationRequestDto notificationRequest);
    Task<ResultAbstract<List<NotificationResponseDto>>> GetUserNotificationsAsync(string customerId);
}
