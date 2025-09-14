using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BT.Application.Services.Interface;
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

    public PayPalService(IOptions<PayPalSettings> payPalSettings)
    {
        _payPalSettings = payPalSettings.Value;

        _environment = new LiveEnvironment(_payPalSettings.ClientId, _payPalSettings.Secret);
        _client = new PayPalHttpClient(_environment);
    }

    public async Task<string> CreateUrlPayment(decimal amount, string description)
    {
        var orderRequest = new OrderRequest()
        {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = "USD",
                        Value = amount.ToString("F2", CultureInfo.InvariantCulture)
                    },
                    Description = description
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

        return result.Links.FirstOrDefault(x => x.Rel == "approve")?.Href;
    }

}