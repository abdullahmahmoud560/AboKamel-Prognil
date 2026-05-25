

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Services.Core.Results;
using System.Net;

namespace Services.Application.ExtensionForServices;

public static class ResolveValidation
{
    public static IServiceCollection ResolveValidationExtension(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .Select(e => new ValidationError
                            {
                                Identifier = e.Key,
                                ErrorCode = HttpStatusCode.BadRequest.ToString(),
                                ErrorMessage = string.Join(" | ", e.Value.Errors.Select(err => err.ErrorMessage))
                            })
                            .ToList();


                var result = Result.Invalid(errors);
                return new BadRequestObjectResult(result);
            };
        });
        return services;
    }
}
