using AboKamel.Application.Contracts.Dashboard.Notifications;
using AboKamel.Application.Dtos.Dashboard.Notifications;
using Capsula.Api.Controllers.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Results;
using System.Security.Claims;

namespace AboKamel.Api.Controllers.Dashboard.Notifications;

public class NotificationsController : DashboardBaseController
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("SendToNotificationEntity")]
    public async Task<ActionResult<ResultAbstract<bool>>> SendNotificationASync([FromBody] NotificationRequestDto dto)
    {
        return await _notificationService.SendNotificationASync(dto);
    }

    [HttpGet("GetUserNotification")]
    [Authorize]
    public async Task<ActionResult<ResultAbstract<List<NotificationResponseDto>>>> GetUserNotificationsASync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _notificationService.GetUserNotificationsAsync(userId);
    }
}
