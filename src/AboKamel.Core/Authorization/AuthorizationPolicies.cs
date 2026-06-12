namespace Services.Core.Authorization;

public static class AuthorizationPolicies
{
    // Policy names as constants for centralized management
    public const string RequireSuperAdmin = "RequireSuperAdmin";
    public const string RequireAdminOrSuperAdmin = "RequireAdminOrSuperAdmin";
    public const string RequirePharmacyAdminOrHigher = "RequirePharmacyAdminOrHigher";
    public const string RequireCustomer = "RequireCustomer";
}
