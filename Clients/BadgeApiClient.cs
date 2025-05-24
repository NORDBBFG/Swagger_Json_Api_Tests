using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class BadgeApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public BadgeApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<BadgeDto>>> GetBadgesAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/badges");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var badges = JsonSerializer.Deserialize<List<BadgeDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<BadgeDto>>(badges, response.StatusCode);
        }

        var errorResult = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new ApiResponse<List<BadgeDto>>(null, response.StatusCode, errorResult);
    }
}

public class ApiResponse<T>
{
    public T Data { get; }
    public System.Net.HttpStatusCode StatusCode { get; }
    public ContentResult ErrorResult { get; }

    public ApiResponse(T data, System.Net.HttpStatusCode statusCode, ContentResult errorResult = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorResult = errorResult;
    }
}