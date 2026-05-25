using AboKamel.Application.Contracts.Dashboard.Notifications;
using AboKamel.Application.Dtos.Dashboard.Notifications;
using AboKamel.Domain.Entities.Notificationns;
using AboKamel.Domain.Repositories.Notifications;
using AutoMapper;
using Services.Core.Results;

namespace AboKamel.Application.Services.Dashboard.Notifications;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<ResultAbstract<List<NotificationResponseDto>>> GetUserNotificationsAsync(string customerId)
    {
        var notifications = await _notificationRepository.GetUserNotificationsAsync(customerId);

        return Result.Success(_mapper.Map<List<NotificationResponseDto>>(notifications));
    }

    public async Task<ResultAbstract<bool>> SendNotificationASync(NotificationRequestDto notificationRequest)
    {
        var Notification = _mapper.Map<Notification>(notificationRequest);
        Notification.CreatedDate = DateTime.UtcNow;
        await _notificationRepository.AddAsync(Notification);
        return Result.Success(true);
    }
}