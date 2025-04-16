using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}

public class BadgeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private bool _simulateError;

    public BadgeApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public void SimulateError(bool simulate)
    {
        _simulateError = simulate;
    }

    public async Task<ApiResponse<List<BadgeDto>>> GetBadgesAsync()
    {
        if (_simulateError)
        {
            return new ApiResponse<List<BadgeDto>>
            {
                StatusCode = 500,
                ErrorMessage = "Simulated Internal Server Error"
            };
        }

        var response = await _httpClient.GetAsync(`${_baseUrl}/api/v1/badges`);
        var statusCode = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            var badges = await response.Content.ReadFromJsonAsync<List<BadgeDto>>();
            return new ApiResponse<List<BadgeDto>>
            {
                Data = badges,
                StatusCode = statusCode
            };
        }
        else
        {
            var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<List<BadgeDto>>
            {
                StatusCode = statusCode,
                ErrorMessage = errorContent?.Content
            };
        }
    }
}