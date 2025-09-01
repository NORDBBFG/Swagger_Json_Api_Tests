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

    public async Task<ApiResponse<byte[]>> GetDonationsXlsxAsync(DonationFilter filter)
    {
        var url = $"{_baseUrl}/api/v1/donation/xlsx";
        var content = new StringContent(JsonConvert.SerializeObject(filter), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var xlsxContent = await response.Content.ReadAsByteArrayAsync();
            return new ApiResponse<byte[]>(xlsxContent, (int)response.StatusCode);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorResult = JsonConvert.DeserializeObject<ContentResult>(errorContent);
            return new ApiResponse<byte[]>(null, (int)response.StatusCode, errorResult);
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; }
    public int StatusCode { get; }
    public ContentResult ErrorResult { get; }

    public ApiResponse(T data, int statusCode, ContentResult errorResult = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorResult = errorResult;
    }
}