using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DonationApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<DonationDto>> CompleteDonationAsync(DonationDto donation)
    {
        var url = `${_baseUrl}/api/v1/user/donation`;
        var content = new StringContent(JsonConvert.SerializeObject(donation), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var donationDto = JsonConvert.DeserializeObject<DonationDto>(responseContent);
            return new ApiResponse<DonationDto>(donationDto, (int)response.StatusCode);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorResult = JsonConvert.DeserializeObject<ContentResult>(errorContent);
            return new ApiResponse<DonationDto>(null, errorResult.StatusCode, errorResult.Content);
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