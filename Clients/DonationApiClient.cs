// DonationApiClient.cs
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DonationApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private bool _simulateError;

    public DonationApiClient(string baseUrl)
    {
        _httpClient = new HttpClient();
        _baseUrl = baseUrl;
    }

    public void SimulateError(bool simulate)
    {
        _simulateError = simulate;
    }

    public async Task<ApiResponse<ContentResult>> AddDonationAsync(DonationDto donation)
    {
        if (_simulateError)
        {
            return new ApiResponse<ContentResult>
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Data = new ContentResult { Content = "Simulated error", StatusCode = 500 }
            };
        }

        var json = JsonConvert.SerializeObject(donation);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/donation", content);

        return await CreateApiResponseAsync<ContentResult>(response);
    }

    public async Task<ApiResponse<ContentResult>> EditDonationAsync(DonationDto donation)
    {
        if (_simulateError)
        {
            return new ApiResponse<ContentResult>
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Data = new ContentResult { Content = "Simulated error", StatusCode = 500 }
            };
        }

        var json = JsonConvert.SerializeObject(donation);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/donation", content);

        return await CreateApiResponseAsync<ContentResult>(response);
    }

    private async Task<ApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response)
    {
        var apiResponse = new ApiResponse<T>
        {
            StatusCode = response.StatusCode,
            Data = default
        };

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            apiResponse.Data = JsonConvert.DeserializeObject<T>(content);
        }

        return apiResponse;
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public T Data { get; set; }
}