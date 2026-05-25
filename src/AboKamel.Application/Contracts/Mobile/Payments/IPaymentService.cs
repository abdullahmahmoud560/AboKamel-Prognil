using Services.Core.DependencyInjection;

namespace Capsula.Application.Contracts.Mobile.Payments;

public interface IPaymentService : IApplicationService, IScopedService
{
    Task<string> CreatePaymentIntentionAsync(string userId);
    Task<string> ProcessPaymentCallback(string payload, string hmacReceived);
}