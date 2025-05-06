using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class DonationTypesApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public DonationTypesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<DonationTypeDto>>> GetDonationTypesAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/donation/types");
        return await CreateApiResponseAsync<List<DonationTypeDto>>(response);
    }

    public async Task<ApiResponse<ContentResult>> AddDonationTypeAsync(DonationTypeDto donationType)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/donation/types", donationType);
        return await CreateApiResponseAsync<ContentResult>(response);
    }

    public async Task<ApiResponse<ContentResult>> EditDonationTypeAsync(DonationTypeDto donationType)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/v1/donation/types", donationType);
        return await CreateApiResponseAsync<ContentResult>(response);
    }

    private async Task<ApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response)
    {
        var apiResponse = new ApiResponse<T>
        {
            StatusCode = response.StatusCode,
            IsSuccessStatusCode = response.IsSuccessStatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = await response.Content.ReadFromJsonAsync<T>();
        }
        else
        {
            apiResponse.ErrorContent = await response.Content.ReadAsStringAsync();
        }

        return apiResponse;
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public bool IsSuccessStatusCode { get; set; }
    public T Data { get; set; }
    public string ErrorContent { get; set; }
}

public class ContentResult
{
    public string Content { get; set; }
    public string ContentType { get; set; }
    public int? StatusCode { get; set; }
}