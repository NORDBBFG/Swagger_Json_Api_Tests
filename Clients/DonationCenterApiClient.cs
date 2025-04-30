using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class DonationCenterApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public DonationCenterApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<DonationCenterDto>>> GetDonationCentersAsync(string donationCenterName = null, int? regionId = null, int? cityId = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrEmpty(donationCenterName))
            query.Add($"donationCenterName={Uri.EscapeDataString(donationCenterName)}");
        if (regionId.HasValue)
            query.Add($"regionId={regionId.Value}");
        if (cityId.HasValue)
            query.Add($"cityId={cityId.Value}");

        var queryString = query.Count > 0 ? "?" + string.Join("&", query) : "";
        var response = await _httpClient.GetAsync($"/api/v1/donationCenters{queryString}");

        return await CreateApiResponseAsync<List<DonationCenterDto>>(response);
    }

    public async Task<ApiResponse<DonationCenterDto>> AddDonationCenterAsync(DonationCenterDto donationCenter)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/donationCenters", donationCenter);
        return await CreateApiResponseAsync<DonationCenterDto>(response);
    }

    private async Task<ApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response)
    {
        var apiResponse = new ApiResponse<T>
        {
            StatusCode = (int)response.StatusCode,
            IsSuccessful = response.IsSuccessStatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = await response.Content.ReadFromJsonAsync<T>();
        }
        else
        {
            apiResponse.ErrorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
        }

        return apiResponse;
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccessful { get; set; }
    public T Data { get; set; }
    public ContentResult ErrorContent { get; set; }
}
