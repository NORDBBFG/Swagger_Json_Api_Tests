using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

    public async Task<ApiResponse<ContentResult>> AddBadgeAsync(BadgeDto badge)
    {
        if (_simulateError)
        {
            return new ApiResponse<ContentResult>
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ContentResult
                {
                    Success = false,
                    Message = "Simulated internal server error",
                    Data = null
                }
            };
        }

        var json = JsonSerializer.Serialize(badge);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/badge", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var contentResult = JsonSerializer.Deserialize<ContentResult>(responseContent);

        return new ApiResponse<ContentResult>
        {
            StatusCode = response.StatusCode,
            Content = contentResult
        };
    }

    public async Task<ApiResponse<ContentResult>> EditBadgeAsync(BadgeDto badge)
    {
        if (_simulateError)
        {
            return new ApiResponse<ContentResult>
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ContentResult
                {
                    Success = false,
                    Message = "Simulated internal server error",
                    Data = null
                }
            };
        }

        var json = JsonSerializer.Serialize(badge);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/badge", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var contentResult = JsonSerializer.Deserialize<ContentResult>(responseContent);

        return new ApiResponse<ContentResult>
        {
            StatusCode = response.StatusCode,
            Content = contentResult
        };
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public T Content { get; set; }
}