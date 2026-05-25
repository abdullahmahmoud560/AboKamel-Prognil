namespace Capsula.Core.Dtos.Payments;

public class PaymobSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string HMAC { get; set; } = string.Empty;
    public string IntegrationCardId { get; set; } = string.Empty;
    public string IFrameId { get; set; } = string.Empty;
    public string IntentionUrl { get; set; } = string.Empty;
    public string CheckoutUrl { get; set; } = string.Empty;
}
