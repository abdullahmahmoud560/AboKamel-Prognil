using Capsula.Application.Contracts.Mobile.Payments;
using Capsula.Application.Services.Mobile.Payments;
using Capsula.Application.Validators.Dashboard.Brands;
using Capsula.Core.Dtos.Payments;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Services.Application.Mappings;
using Services.Core.DependencyInjection;
using Services.Infrastructure.DbContexts;
using Services.Infrastructure.ExtensionMethods;
using System.Reflection;
using System.Text.Json.Serialization;


namespace Services.Application.ExtensionForServices;
/// <summary>
/// A static class providing extension methods for adding services to the application.
/// </summary>
public static class ServicesRegisteration
{
    /// <summary>
    /// Adds services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection with Parkilo services added.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddRepositoryServices();
        var serviceTypes = Assembly.GetExecutingAssembly().ExportedTypes
            .Where(t => t.IsAssignableTo(typeof(IApplicationService)) && !t.IsInterface && !t.IsAbstract).ToList();

        foreach (var serviceType in serviceTypes)
        {
            var interfaceType = serviceType.GetInterface("I" + serviceType.Name);
            var result = serviceType switch
            {
                _ when typeof(IScopedService).IsAssignableFrom(serviceType) => services.AddScoped(interfaceType, serviceType),
                _ when typeof(ITransientService).IsAssignableFrom(serviceType) => services.AddTransient(interfaceType, serviceType),
                _ when typeof(ISingletonService).IsAssignableFrom(serviceType) => services.AddSingleton(interfaceType, serviceType),
            };
        }
        return services;
    }
    /// <summary>
    /// Adds AutoMapper, FluentValidation, and services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection with the added services.</returns>
    public static IServiceCollection ResolveServicesExtension(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {

        services.AddControllers();
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //});
        services.Configure<PaymobSettings>(configuration.GetSection("Paymob"));
        services.AddHttpClient<IPaymentService, PaymentService>();
        services.AddFluentValidationAutoValidation(fv =>
        {
            fv.DisableDataAnnotationsValidation = true;
        });
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<BrandValidator>();
        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.ResolveValidationExtension();
        services.AddServices();
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "AboKamel API", Version = "v1" });

            // Add JWT Authentication
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token."
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new string[] {}
                }
             });
        });

        return services;
    }
}
