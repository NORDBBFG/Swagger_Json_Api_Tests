using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DonationCenterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationCenterApiClient(string baseUrl, string bearerToken)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
    }

    public async Task<ApiResponse<List<DonationCenterDto>>> GetDonationCentersAsync(string donationCenterName, int regionId, int cityId)
    {
        var url = $"{_baseUrl}/api/v1/donationCenters?donationCenterName={Uri.EscapeDataString(donationCenterName)}&regionId={regionId}&cityId={cityId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var donationCenters = JsonConvert.DeserializeObject<List<DonationCenterDto>>(content);
            return new ApiResponse<List<DonationCenterDto>>(donationCenters, (int)response.StatusCode);
        }
        else
        {
            var errorContent = JsonConvert.DeserializeObject<ContentResult>(content);
            return new ApiResponse<List<DonationCenterDto>>(null, (int)response.StatusCode, errorContent?.Content);
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }

    public ApiResponse(T data, int statusCode, string errorMessage = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}