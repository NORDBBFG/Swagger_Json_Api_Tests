using System;
using System.Net.Http;
using System.Text;
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

    public async Task<ApiResponse<ContentResult>> AddBadgeAsync(BadgeDto badge)
    {
        if (_simulateError)
        {
            return new ApiResponse<ContentResult>
            {
                StatusCode = 500,
                ErrorMessage = "Simulated server error"
            };
        }

        var json = JsonConvert.SerializeObject(badge);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/badge", content);

        return await ProcessResponseAsync<ContentResult>(response);
    }

    public async Task<ApiResponse<ContentResult>> EditBadgeAsync(BadgeDto badge)
    {
        if (_simulateError)
        {
            return new ApiResponse<ContentResult>
            {
                StatusCode = 500,
                ErrorMessage = "Simulated server error"
            };
        }

        var json = JsonConvert.SerializeObject(badge);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/badge", content);

        return await ProcessResponseAsync<ContentResult>(response);
    }

    private async Task<ApiResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
    {
        var apiResponse = new ApiResponse<T>
        {
            StatusCode = (int)response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            apiResponse.Data = JsonConvert.DeserializeObject<T>(responseContent);
        }
        else
        {
            apiResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
        }

        return apiResponse;
    }
}