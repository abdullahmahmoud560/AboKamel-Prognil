using AboKamel.Application.Dtos.Dashboard.Notifications;
using AboKamel.Domain.Entities.Notificationns;
using Services.Application.Mappings;

namespace AboKamel.Application.Mappings.Dashboard.Notifications;

public static class NotificationMapping
{
    public static void AddNotificationMapping(this MappingProfiles map)
    {
        map.CreateMap<NotificationRequestDto, Notification>();
        map.CreateMap<Notification, NotificationResponseDto>();
    }
}
