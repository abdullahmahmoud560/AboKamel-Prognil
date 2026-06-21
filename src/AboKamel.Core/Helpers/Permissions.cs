namespace Services.Core.Helpers;

public static class Permissions
{
    // Driver permissions
    public const string ViewInvoices = "Permissions.Driver.ViewInvoices";
    public const string ViewInvoicePhoneNumber = "Permissions.Driver.ViewInvoicePhoneNumber";
    public const string ViewInvoiceLocation = "Permissions.Driver.ViewInvoiceLocation";
    public const string ViewInvoiceGeneralData = "Permissions.Driver.ViewInvoiceGeneralData";
    public const string DeliverOrders = "Permissions.Driver.DeliverOrders";
    
    // Salesperson permissions
    public const string RegisterNewAccounts = "Permissions.Salesperson.RegisterNewAccounts";
}
