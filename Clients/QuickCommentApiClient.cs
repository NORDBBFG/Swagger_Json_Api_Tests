using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}

public class QuickCommentApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private bool _simulateError;

    public QuickCommentApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public void SimulateError(bool simulate)
    {
        _simulateError = simulate;
    }

    public async Task<ApiResponse<List<QuickCommentDto>>> GetQuickCommentsAsync(QuickCommentCategory category)
    {
        if (_simulateError)
        {
            return new ApiResponse<List<QuickCommentDto>>
            {
                StatusCode = 500,
                ErrorMessage = "Simulated internal server error"
            };
        }

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/QuickComments?quickCommentCategory={(int)category}", null);

        var apiResponse = new ApiResponse<List<QuickCommentDto>>
        {
            StatusCode = (int)response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            apiResponse.Data = JsonSerializer.Deserialize<List<QuickCommentDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            apiResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
        }

        return apiResponse;
    }
}
