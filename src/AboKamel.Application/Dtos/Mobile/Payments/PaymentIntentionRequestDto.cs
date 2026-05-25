using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Payments;

public class PaymentIntentionRequestDto
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "EGP";

    [JsonPropertyName("payment_methods")]
    public List<string> PaymentMethods { get; set; } = new();

    [JsonPropertyName("items")]
    public List<PaymobItemDto> Items { get; set; } = new();

    [JsonPropertyName("billing_data")]
    public PaymobBillingDataDto BillingData { get; set; } = new();

    [JsonPropertyName("extras")]
    public Dictionary<string, object> Extras { get; set; } = new Dictionary<string, object>();

    [JsonPropertyName("special_reference")]
    public string? SpecialReference { get; set; }

    [JsonPropertyName("expiration")]
    public int Expiration { get; set; } = 3600;

    [JsonPropertyName("notification_url")]
    public string? NotificationUrl { get; set; } = "N/A";

    [JsonPropertyName("redirection_url")]
    public string? RedirectionUrl { get; set; } = "N/A";
}
