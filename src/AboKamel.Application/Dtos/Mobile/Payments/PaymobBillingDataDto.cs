using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Payments;

public class PaymobBillingDataDto
{
    [JsonPropertyName("apartment")]
    public string Apartment { get; set; } = "";

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = "";

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = "";

    [JsonPropertyName("street")]
    public string Street { get; set; } = "";

    [JsonPropertyName("building")]
    public string Building { get; set; } = "";

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = "N/A";

    [JsonPropertyName("city")]
    public string City { get; set; } = "";

    [JsonPropertyName("country")]
    public string Country { get; set; } = "";

    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("floor")]
    public string Floor { get; set; } = "";

    [JsonPropertyName("state")]
    public string State { get; set; } = "";
}
