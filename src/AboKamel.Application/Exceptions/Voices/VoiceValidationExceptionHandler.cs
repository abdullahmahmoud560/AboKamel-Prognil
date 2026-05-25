using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

namespace Capsula.Application.Exceptions.Voices;

public class VoiceValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<VoiceValidationExceptionHandler> _logger;

    public VoiceValidationExceptionHandler(ILogger<VoiceValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is VoiceValidationException ex)
        {
            _logger.LogWarning("Voice validation failed: {Message}", ex.Message);

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(Result.Error(ex.Message));

            return true;
        }

        return false;
    }
}
