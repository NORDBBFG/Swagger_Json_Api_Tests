using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

    public BadgeApiClient(string baseUrl)
    {
        _httpClient = new HttpClient();
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

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/badges");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var badges = JsonConvert.DeserializeObject<List<BadgeDto>>(content);
            return new ApiResponse<List<BadgeDto>>
            {
                Data = badges,
                StatusCode = (int)response.StatusCode
            };
        }
        else
        {
            return new ApiResponse<List<BadgeDto>>
            {
                StatusCode = (int)response.StatusCode,
                ErrorMessage = content
            };
        }
    }
}