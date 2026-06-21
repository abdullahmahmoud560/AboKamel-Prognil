using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Services.Core.Authorization;
using Services.Core.Helpers;
using Services.Core.Helpers.Roles;

namespace Services.Application.ExtensionForServices;

public static class AuthorizationPoliciesConfiguration
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // SuperAdmin only policy
            options.AddPolicy(AuthorizationPolicies.RequireSuperAdmin, policy =>
                policy.RequireRole(RoleName.SuperAdmin));

            // Driver only policy
            options.AddPolicy(AuthorizationPolicies.RequireDriver, policy =>
                policy.RequireRole(RoleName.SuperAdmin, RoleName.Driver));

            // Salesperson only policy
            options.AddPolicy(AuthorizationPolicies.RequireSalesperson, policy =>
                policy.RequireRole(RoleName.SuperAdmin, RoleName.Salesperson));

            // Customer only policy
            options.AddPolicy(AuthorizationPolicies.RequireCustomer, policy =>
                policy.RequireRole(RoleName.SuperAdmin, RoleName.Customer));
            
            // Permission policies - allow SuperAdmin as well
            options.AddPolicy(AuthorizationPolicies.RequireViewInvoices, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(RoleName.SuperAdmin) ||
                    context.User.HasClaim("Permission", Permissions.ViewInvoices)));
            options.AddPolicy(AuthorizationPolicies.RequireViewInvoicePhoneNumber, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(RoleName.SuperAdmin) ||
                    context.User.HasClaim("Permission", Permissions.ViewInvoicePhoneNumber)));
            options.AddPolicy(AuthorizationPolicies.RequireViewInvoiceLocation, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(RoleName.SuperAdmin) ||
                    context.User.HasClaim("Permission", Permissions.ViewInvoiceLocation)));
            options.AddPolicy(AuthorizationPolicies.RequireViewInvoiceGeneralData, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(RoleName.SuperAdmin) ||
                    context.User.HasClaim("Permission", Permissions.ViewInvoiceGeneralData)));
            options.AddPolicy(AuthorizationPolicies.RequireDeliverOrders, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(RoleName.SuperAdmin) ||
                    context.User.HasClaim("Permission", Permissions.DeliverOrders)));
            options.AddPolicy(AuthorizationPolicies.RequireRegisterNewAccounts, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(RoleName.SuperAdmin) ||
                    context.User.HasClaim("Permission", Permissions.RegisterNewAccounts)));
        });

        return services;
    }
}
