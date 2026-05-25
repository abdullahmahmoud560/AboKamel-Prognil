using Microsoft.AspNetCore.SignalR;

namespace AboKamel.Application.Services.Hubs;

public class NotificationHub : Hub
{
    // Optional: server method clients can call
    public async Task SendFromClient(string message)
    {
        // Broadcast incoming client message to everyone
        await Clients.All.SendAsync("ReceiveNotification", new
        {
            Message = message,
            FromConnectionId = Context.ConnectionId
        });
    }

    public override Task OnConnectedAsync()
    {
        // optional: log or track connected users
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(System.Exception? exception)
    {
        // optional: cleanup
        return base.OnDisconnectedAsync(exception);
    }
}
