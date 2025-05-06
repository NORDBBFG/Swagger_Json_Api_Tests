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
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<DonationTypeDto>>> GetDonationTypesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/v1/donation/types");
            response.EnsureSuccessStatusCode();
            var donationTypes = await response.Content.ReadFromJsonAsync<List<DonationTypeDto>>();
            return new ApiResponse<List<DonationTypeDto>>(donationTypes, (int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<List<DonationTypeDto>>(null, 500, ex.Message);
        }
    }

    public async Task<ApiResponse<ContentResult>> AddDonationTypeAsync(DonationTypeDto donationType)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/donation/types", donationType);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<ContentResult>(content, (int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<ContentResult>(null, 500, ex.Message);
        }
    }

    public async Task<ApiResponse<ContentResult>> EditDonationTypeAsync(DonationTypeDto donationType)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("/api/v1/donation/types", donationType);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<ContentResult>(content, (int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<ContentResult>(null, 500, ex.Message);
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; }
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public ApiResponse(T data, int statusCode, string errorMessage = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}