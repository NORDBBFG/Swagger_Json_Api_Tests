using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<BadgeDto>> GetBadgeAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/badge");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var badge = JsonConvert.DeserializeObject<BadgeDto>(content);
            return new ApiResponse<BadgeDto>(badge, response.StatusCode);
        }
        else
        {
            var errorContent = JsonConvert.DeserializeObject<ContentResult>(content);
            return new ApiResponse<BadgeDto>(null, response.StatusCode, errorContent);
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; }
    public System.Net.HttpStatusCode StatusCode { get; }
    public ContentResult ErrorContent { get; }

    public ApiResponse(T data, System.Net.HttpStatusCode statusCode, ContentResult errorContent = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }
}