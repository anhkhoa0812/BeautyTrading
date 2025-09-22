using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BT.Application.Services.Interface;
using BT.Domain.Models.Payment;
using BT.Domain.Models.Settings;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace BT.Application.Services.Implement;

public class PayPalService : IPayPalService
{
    private readonly PayPalSettings _payPalSettings;
    private readonly PayPalEnvironment _environment;
    private readonly PayPalHttpClient _client;
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public PayPalService(IOptions<PayPalSettings> payPalSettings, HttpClient httpClient, ILogger logger)
    {
        _payPalSettings = payPalSettings.Value;

        _environment = new LiveEnvironment(_payPalSettings.ClientId, _payPalSettings.Secret);
        _client = new PayPalHttpClient(_environment);
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PayPalCreateOrder> CreateUrlPayment(Domain.Entities.Order order, string currency, string description, decimal vat, decimal shipping)
    {
        var itemTotal = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

        var vatAmount = vat > 0 
            ? Math.Round(((itemTotal + shipping) * vat) / 100m, 2) 
            : 0m;
        
        var total = itemTotal + shipping + vatAmount;
        
        var orderRequest = new OrderRequest()
        {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = currency,
                        Value = total.ToString("F2", CultureInfo.InvariantCulture),
                        AmountBreakdown = new AmountBreakdown
                        {
                            ItemTotal = new Money
                            {
                                CurrencyCode = currency,
                                Value = itemTotal.ToString("F2", CultureInfo.InvariantCulture)
                            },
                            TaxTotal = new Money()
                            {
                                CurrencyCode = currency,
                                Value = vatAmount
                                    .ToString("F2", CultureInfo.InvariantCulture)
                            },
                            Shipping = new Money()
                            {
                                CurrencyCode = currency,
                                Value = shipping
                                    .ToString("F2", CultureInfo.InvariantCulture)
                            },
                        }
                    },
                    Description = description,
                    Items = order.OrderItems.Select(oi => new Item
                    {
                        Name = $"{oi.ProductVariant?.Name ?? "No Variant"} - {oi.ProductColor?.ColorName ?? "No Color"}",
                        Quantity = oi.Quantity.ToString(),
                        UnitAmount = new Money
                        {
                            CurrencyCode = currency,
                            Value = oi.Price.ToString("F2", CultureInfo.InvariantCulture)
                        }
                    }).ToList()
                }
            },
            ApplicationContext = new ApplicationContext
            {
                ReturnUrl = _payPalSettings.ReturnUrl,
                CancelUrl = _payPalSettings.CancelUrl
            }
        };

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(orderRequest);

        var response = await _client.Execute(request);
        var result = response.Result<Order>();
        
        var lastOrderId = result.Id;

        return new PayPalCreateOrder()
        {
            Code = lastOrderId,
            Url = result.Links.FirstOrDefault(x => x.Rel == "approve")?.Href
        };
    }

    public async Task<bool> VerifyWebhookAsync(string body, IHeaderDictionary headers)
    {
        var verifyRequest = new
        {
            auth_algo = headers["PAYPAL-AUTH-ALGO"].ToString(),
            cert_url = headers["PAYPAL-CERT-URL"].ToString(),
            transmission_id = headers["PAYPAL-TRANSMISSION-ID"].ToString(),
            transmission_sig = headers["PAYPAL-TRANSMISSION-SIG"].ToString(),
            transmission_time = headers["PAYPAL-TRANSMISSION-TIME"].ToString(),
            webhook_id = _payPalSettings.WebhookId,
            webhook_event = JsonSerializer.Deserialize<JsonElement>(body)
        };

        _logger.Information($"PayPal verify webhook: {body}");
        
        var requestContent = new StringContent(
            JsonSerializer.Serialize(verifyRequest),
            Encoding.UTF8,
            "application/json"
        );
        
        _logger.Information($"PayPal verify webhook request: {requestContent}");

        var url = "https://api-m.paypal.com/v1/notifications/verify-webhook-signature";
        var accessToken = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        
        _logger.Information($"PayPal verify webhook access token: {accessToken}");

        var response = await _httpClient.PostAsync(url, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        _logger.Information($"PayPal verify webhook response: {responseContent}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"PayPal verify webhook failed: {response.StatusCode} - {responseContent}");
        }

        using var doc = JsonDocument.Parse(responseContent);
        if (doc.RootElement.TryGetProperty("verification_status", out var status))
        {
            return status.GetString()?.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase) == true;
        }

        return true;
    }
    
    private async Task<string> GetAccessTokenAsync()
    {
        var url = "https://api-m.paypal.com/v1/oauth2/token";

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(
            $"{_payPalSettings.ClientId}:{_payPalSettings.Secret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get PayPal access token: {response.StatusCode} - {content}");
        }

        using var doc = JsonDocument.Parse(content);
        return doc.RootElement.GetProperty("access_token").GetString();
    }
}