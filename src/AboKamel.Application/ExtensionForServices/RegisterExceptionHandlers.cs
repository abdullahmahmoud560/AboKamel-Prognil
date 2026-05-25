using Microsoft.Extensions.DependencyInjection;

namespace Capsula.Application.ExtensionForServices;

public static class RegisterExceptionHandlers
{
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<ImageValidationExceptionHandler>();

        return services;
    }
}
