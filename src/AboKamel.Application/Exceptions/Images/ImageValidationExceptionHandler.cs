using Capsula.Application.Exceptions.Images;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

public class ImageValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ImageValidationExceptionHandler> _logger;

    public ImageValidationExceptionHandler(ILogger<ImageValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ImageValidationException ex)
        {
            _logger.LogWarning("Image validation failed: {Message}", ex.Message);

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(Result.Error(ex.Message));

            return true; // exception handled
        }

        return false; // let others handle
    }
}