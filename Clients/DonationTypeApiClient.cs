using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DonationTypeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public DonationTypeApiClient(string baseUrl, string token)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public async Task<ApiResponse<ContentResult>> UpdateDonationTypeAsync(DonationTypeDto donationTypeDto)
    {
        var json = JsonConvert.SerializeObject(donationTypeDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/donation/types", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var contentResult = JsonConvert.DeserializeObject<ContentResult>(responseContent);

        return new ApiResponse<ContentResult>
        {
            StatusCode = (int)response.StatusCode,
            Data = contentResult
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
}