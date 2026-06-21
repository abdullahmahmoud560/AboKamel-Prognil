namespace Services.Core.Authorization;

public static class AuthorizationPolicies
{
    // Policy names as constants for centralized management
    public const string RequireSuperAdmin = "RequireSuperAdmin";
    public const string RequireDriver = "RequireDriver";
    public const string RequireSalesperson = "RequireSalesperson";
    public const string RequireCustomer = "RequireCustomer";
    
    // Permission policies
    public const string RequireViewInvoices = "RequireViewInvoices";
    public const string RequireViewInvoicePhoneNumber = "RequireViewInvoicePhoneNumber";
    public const string RequireViewInvoiceLocation = "RequireViewInvoiceLocation";
    public const string RequireViewInvoiceGeneralData = "RequireViewInvoiceGeneralData";
    public const string RequireDeliverOrders = "RequireDeliverOrders";
    public const string RequireRegisterNewAccounts = "RequireRegisterNewAccounts";
}
