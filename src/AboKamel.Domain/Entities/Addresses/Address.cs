using Capsula.Domain.Entities.Users.Customers;
using Services.Core.Entities;

namespace Capsula.Domain.Entities.Addresses;

public class Address : BaseEntity<int>
{
    public string? Region { get; set; } = string.Empty; // e.g., "مدينة نصر"

    public string? BuildingName { get; set; } = string.Empty;

    public string? ApartmentNumber { get; set; } = string.Empty;

    public string? FloorNumber { get; set; }
    
    public string PhoneNumber { get; set; } = string.Empty;
    public string DetailedAddress { get; set; } = string.Empty;

    public string? DeliveryInstructions { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool IsPrimary { get; set; } = false;

    public string CustomerId { get; set; } = string.Empty;
    public Customer Customer { get; set; }
}