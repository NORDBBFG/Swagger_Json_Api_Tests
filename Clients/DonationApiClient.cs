using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class DonationApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private bool _simulateError;

    public DonationApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public void SimulateError(bool simulate)
    {
        _simulateError = simulate;
    }

    public async Task<ApiResponse<ContentResult>> ConfirmDonationAsync(DonationDto donation)
    {
        if (_simulateError)
        {
            return new ApiResponse<ContentResult>
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ContentResult
                {
                    Content = "Simulated server error",
                    ContentType = "application/json",
                    StatusCode = 500
                }
            };
        }

        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/v1/donation/confirm", donation);

        var content = await response.Content.ReadFromJsonAsync<ContentResult>();

        return new ApiResponse<ContentResult>
        {
            StatusCode = response.StatusCode,
            Content = content
        };
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public T Content { get; set; }
}
