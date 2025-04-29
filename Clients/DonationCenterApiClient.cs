using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class DonationCenterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationCenterApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl.TrimEnd('/');
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

        var url = $"{_baseUrl}/api/v1/donationCenters";
        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var donationCenters = JsonSerializer.Deserialize<List<DonationCenterDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, donationCenters);
        }

        return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, null, content);
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public string ErrorMessage { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data, string errorMessage = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
    }
}