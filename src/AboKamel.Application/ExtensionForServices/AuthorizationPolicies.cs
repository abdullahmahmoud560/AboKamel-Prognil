using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Services.Core.Authorization;
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

            // Admin or SuperAdmin policy
            options.AddPolicy(AuthorizationPolicies.RequireAdminOrSuperAdmin, policy =>
                policy.RequireRole(RoleName.Admin, RoleName.SuperAdmin));

            // PharmacyAdmin or higher (Admin, SuperAdmin) policy
            options.AddPolicy(AuthorizationPolicies.RequirePharmacyAdminOrHigher, policy =>
                policy.RequireRole(RoleName.PharmacyAdmin, RoleName.Admin, RoleName.SuperAdmin));

            // Customer only policy
            options.AddPolicy(AuthorizationPolicies.RequireCustomer, policy =>
                policy.RequireRole(RoleName.Customer));
        });

        return services;
    }
}
