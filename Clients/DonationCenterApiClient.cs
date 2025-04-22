using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DonationCenterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationCenterApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<DonationCenterDto>> AddDonationCenterAsync(DonationCenterDto donationCenter)
    {
        var url = $"{_baseUrl}/api/v1/donationCenters";
        var content = new StringContent(JsonConvert.SerializeObject(donationCenter), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdDonationCenter = JsonConvert.DeserializeObject<DonationCenterDto>(responseContent);
            return new ApiResponse<DonationCenterDto>(response.StatusCode, createdDonationCenter);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<DonationCenterDto>(response.StatusCode, null, errorContent);
        }
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public string ErrorContent { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data, string errorContent = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorContent = errorContent;
    }
}