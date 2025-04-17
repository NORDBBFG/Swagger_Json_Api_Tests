using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class FirebaseApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public FirebaseApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<ContentResult>> SavePushTokenAsync(string deviceId, string token)
    {
        var response = await _httpClient.PostAsync($"/api/v1/Firebase?deviceId={Uri.EscapeDataString(deviceId)}&token={Uri.EscapeDataString(token)}", null);

        var content = await response.Content.ReadAsStringAsync();
        var contentResult = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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