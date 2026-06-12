using AboKamel.Application.Services.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Api.Controllers.Mobile;
using Services.Core.Helpers.Roles;

namespace AboKamel.Api.Controllers.Mobile.Notifications;

[Authorize(Roles = RoleName.SuperAdmin)]
public class NotificationsController : MobileBaseController
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationsController(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public class NotificationDto
    {
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public object? Data { get; set; }
    }

    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastToAll([FromBody] NotificationDto dto)
    {
        // Send to all connected clients
        await _hub.Clients.All.SendAsync("ReceiveNotification", dto);
        return Ok(new { Sent = true });
    }
}
