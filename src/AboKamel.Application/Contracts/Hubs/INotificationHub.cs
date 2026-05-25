namespace AboKamel.Application.Contracts.Hubs;

public interface INotificationHub
{
    Task SendMessageToAllUsers(string Message);
}