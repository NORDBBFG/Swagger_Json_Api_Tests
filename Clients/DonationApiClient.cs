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

    public async Task<ApiResponse<ContentResult>> AddDonationAsync(DonationDto donation)
    {
        var json = JsonConvert.SerializeObject(donation);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/donation", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var contentResult = JsonConvert.DeserializeObject<ContentResult>(responseContent);

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = contentResult
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
}