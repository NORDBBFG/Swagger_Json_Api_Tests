using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}

public class FaqApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private bool _simulateError;

    public FaqApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public void SimulateError(bool simulate)
    {
        _simulateError = simulate;
    }

    public async Task<ApiResponse<List<FaqArticleDto>>> FilterFaqsAsync(FaqFilter filter)
    {
        if (_simulateError)
        {
            return new ApiResponse<List<FaqArticleDto>>
            {
                StatusCode = 500,
                ErrorMessage = "Simulated internal server error"
            };
        }

        var json = JsonSerializer.Serialize(filter);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/faqs/filter", content);

        var apiResponse = new ApiResponse<List<FaqArticleDto>>
        {
            StatusCode = (int)response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            apiResponse.Data = JsonSerializer.Deserialize<List<FaqArticleDto>>(responseContent);
        }
        else
        {
            apiResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
        }

        return apiResponse;
    }
}