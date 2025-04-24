using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class DonationCenterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationCenterApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
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
            var donationCenters = JsonSerializer.Deserialize<List<DonationCenterDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, donationCenters);
        }
        else
        {
            var errorContent = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, null, errorContent);
        }
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public ContentResult ErrorContent { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data, ContentResult errorContent = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorContent = errorContent;
    }
}

public class ContentResult
{
    public string Content { get; set; }
    public string ContentType { get; set; }
    public int? StatusCode { get; set; }
}