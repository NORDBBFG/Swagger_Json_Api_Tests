using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DonationCenterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationCenterApiClient(string baseUrl, string token)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
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

        var url = $"{_baseUrl}/api/v1/donationCenters{(query.Count > 0 ? "?" + string.Join("&", query) : "")}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var donationCenters = JsonConvert.DeserializeObject<List<DonationCenterDto>>(content);
            return new ApiResponse<List<DonationCenterDto>>(donationCenters, (int)response.StatusCode);
        }
        else
        {
            var errorResult = JsonConvert.DeserializeObject<ContentResult>(content);
            return new ApiResponse<List<DonationCenterDto>>(null, errorResult.StatusCode, errorResult.Content);
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