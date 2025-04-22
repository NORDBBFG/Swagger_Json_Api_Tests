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

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<List<DonationCenterDto>>();
            return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, content);
        }
        else
        {
            var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, null, errorContent);
        }
    }

    public async Task<ApiResponse<DonationCenterDto>> AddDonationCenterAsync(DonationCenterDto donationCenter)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/donationCenters", donationCenter);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<DonationCenterDto>();
            return new ApiResponse<DonationCenterDto>(response.StatusCode, content);
        }
        else
        {
            var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<DonationCenterDto>(response.StatusCode, null, errorContent);
        }
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public ContentResult Error { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data, ContentResult error = null)
    {
        StatusCode = statusCode;
        Data = data;
        Error = error;
    }
}