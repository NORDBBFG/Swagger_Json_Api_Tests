using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class DonationTypesApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationTypesApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public async Task<ApiResponse<List<DonationTypeDto>>> GetDonationTypesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/donation/types");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var donationTypes = JsonSerializer.Deserialize<List<DonationTypeDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<DonationTypeDto>>
            {
                Data = donationTypes,
                StatusCode = (int)response.StatusCode
            };
        }
        else
        {
            var errorContent = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<DonationTypeDto>>
            {
                Error = errorContent,
                StatusCode = (int)response.StatusCode
            };
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; set; }
    public ContentResult Error { get; set; }
    public int StatusCode { get; set; }
}