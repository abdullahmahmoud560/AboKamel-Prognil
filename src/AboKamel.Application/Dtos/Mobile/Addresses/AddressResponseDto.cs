using Capsula.Core.Enums;
using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Addresses;

public class AddressResponseDto : BaseResponseDto<int>
{
    public string? Region { get; set; } = string.Empty;

    public string? BuildingName { get; set; } = string.Empty;

    public string? ApartmentNumber { get; set; } = string.Empty;

    public string? FloorNumber { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;
    public string DetailedAddress { get; set; } = string.Empty;

    public string? DeliveryInstructions { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool IsPrimary { get; set; } = false;
}
