using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class BadgeApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public BadgeApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<ContentResult>> GetBadgeAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/badge");
        var content = await response.Content.ReadAsStringAsync();

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = JsonConvert.DeserializeObject<ContentResult>(content)
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
}