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

    public async Task<ApiResponse<List<DonationDto>>> GetDonationsAsync(DonationFilter filter)
    {
        if (_simulateError)
        {
            return new ApiResponse<List<DonationDto>>
            {
                StatusCode = 500,
                ErrorMessage = "Simulated internal server error"
            };
        }

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/donations", filter);

        if (response.IsSuccessStatusCode)
        {
            var donations = await response.Content.ReadFromJsonAsync<List<DonationDto>>();
            return new ApiResponse<List<DonationDto>>
            {
                Data = donations,
                StatusCode = (int)response.StatusCode
            };
        }
        else
        {
            var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<List<DonationDto>>
            {
                StatusCode = (int)response.StatusCode,
                ErrorMessage = errorContent.Content
            };
        }
    }
}