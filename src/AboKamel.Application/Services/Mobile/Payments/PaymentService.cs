using Capsula.Application.Contracts.Mobile.Carts;
using Capsula.Application.Contracts.Mobile.Payments;
using Capsula.Core.Dtos.Payments;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Capsula.Application.Services.Mobile.Payments;

public class PaymentService : IPaymentService
{
    private readonly PaymobSettings _paymobSettings;
    private readonly ICartService _cartService;
    private readonly HttpClient _httpClient;

    public PaymentService(IOptions<PaymobSettings> paymobSettings, ICartService cartService, HttpClient httpClient)
    {
        _paymobSettings = paymobSettings.Value;
        _cartService = cartService;
        _httpClient = httpClient;
    }

    public async Task<string> CreatePaymentIntentionAsync(string userId)
    {
        var result = await _cartService.GetCustomerCartDetailsAsync(userId);

        var cart = result.Value;

        var paymobRequest = cart.ToPaymobIntention(_paymobSettings.IntegrationCardId);

        var json = JsonSerializer.Serialize(paymobRequest);

        var request = new HttpRequestMessage(HttpMethod.Post, _paymobSettings.IntentionUrl)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Token", _paymobSettings.SecretKey);

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        string clientSecret = JsonNode.Parse(responseBody)?["client_secret"]?.ToString() ?? string.Empty;

        var intentionUrl = $"{_paymobSettings.CheckoutUrl}publicKey={_paymobSettings.PublicKey}&clientSecret={clientSecret}";

        return intentionUrl;
    }

    public Task<string> ProcessPaymentCallback(string payload, string hmacReceived)
    {
        throw new NotImplementedException();
    }
}
