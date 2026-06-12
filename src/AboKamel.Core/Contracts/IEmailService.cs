using Services.Core.DependencyInjection;
using System.Threading.Tasks;

namespace Services.Core.Contracts;

public interface IEmailService : IApplicationService, IScopedService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
