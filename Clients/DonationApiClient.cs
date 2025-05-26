using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DonationApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public DonationApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<HttpResponseMessage> GetDonationsXlsxAsync(DonationFilter filter)
    {
        var json = JsonConvert.SerializeObject(filter);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return await _httpClient.PostAsync("/api/v1/donation/xlsx", content);
    }
}