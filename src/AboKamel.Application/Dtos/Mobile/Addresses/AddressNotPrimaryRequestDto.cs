using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Addresses;

public class AddressNotPrimaryRequestDto : BaseRequestDto
{
    public string? Region { get; set; }

    public string? BuildingName { get; set; }

    public string? ApartmentNumber { get; set; }

    public string? FloorNumber { get; set; }

    public string? PhoneNumber { get; set; }

    public string? DetailedAddress { get; set; }

    public string? DeliveryInstructions { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    [JsonIgnore]
    public string? CustomerId { get; set; }
}