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
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var createdDonationCenter = JsonConvert.DeserializeObject<DonationCenterDto>(responseContent);
            return new ApiResponse<DonationCenterDto>(response.StatusCode, createdDonationCenter);
        }
        else
        {
            var errorContent = JsonConvert.DeserializeObject<ContentResult>(responseContent);
            return new ApiResponse<DonationCenterDto>(response.StatusCode, errorContent);
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