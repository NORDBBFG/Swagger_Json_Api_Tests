using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class BadgeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public BadgeApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public async Task<ApiResponse<List<BadgeDto>>> GetBadgesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/badges");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var badges = JsonSerializer.Deserialize<List<BadgeDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<BadgeDto>>(badges, (int)response.StatusCode);
        }
        else
        {
            var errorContent = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<BadgeDto>>(default, (int)response.StatusCode, errorContent);
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; }
    public int StatusCode { get; }
    public ContentResult ErrorContent { get; }

    public ApiResponse(T data, int statusCode, ContentResult errorContent = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }
}