using System.Text.Json;
using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Models.Common;
using BT.Domain.Models.Settings;
using BT.Domain.Models.Vat;
using Microsoft.Extensions.Options;

namespace BT.Application.Services.Implement;

public class VatCheckService : IVatCheckService
{
    private readonly HttpClient _client;
    private readonly VatcheckApiSettings _vatcheckApiSettings;

    public VatCheckService(HttpClient client, IOptions<VatcheckApiSettings> vatcheckApiSettings)
    {
        _client = client;
        _vatcheckApiSettings = vatcheckApiSettings.Value;
    }
    
    public async Task<VatCheckResponse> CheckVat(string vatNumber)
    {
        var url = $"{_vatcheckApiSettings.Url}/validate/{vatNumber}?apikey={_vatcheckApiSettings.ApiKey}";
        var response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API error: {response.StatusCode} - {error}");
        }

        var content = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<VatCheckResponse>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return result!;
    }
}