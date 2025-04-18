using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Web;

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
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrEmpty(donationCenterName))
            query["donationCenterName"] = donationCenterName;
        if (regionId.HasValue)
            query["regionId"] = regionId.Value.ToString();
        if (cityId.HasValue)
            query["cityId"] = cityId.Value.ToString();

        var requestUrl = $"{_baseUrl}/api/v1/donationCenters?{query}";

        var response = await _httpClient.GetAsync(requestUrl);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var donationCenters = JsonSerializer.Deserialize<List<DonationCenterDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, donationCenters);
        }
        else
        {
            var errorContent = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<DonationCenterDto>>(response.StatusCode, errorContent);
        }
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public ContentResult ErrorContent { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data)
    {
        StatusCode = statusCode;
        Data = data;
    }

    public ApiResponse(System.Net.HttpStatusCode statusCode, ContentResult errorContent)
    {
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }
}