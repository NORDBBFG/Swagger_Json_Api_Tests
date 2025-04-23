using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class QuickCommentsApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public QuickCommentsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<QuickCommentDto>>> GetQuickCommentsAsync(QuickCommentCategory category)
    {
        var response = await _httpClient.PostAsync($"/api/v1/QuickComments?quickCommentCategory={(int)category}", null);

        var apiResponse = new ApiResponse<List<QuickCommentDto>>
        {
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers,
            IsSuccessStatusCode = response.IsSuccessStatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            apiResponse.Data = JsonSerializer.Deserialize<List<QuickCommentDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            apiResponse.ErrorContent = await response.Content.ReadAsStringAsync();
        }

        return apiResponse;
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public HttpResponseHeaders Headers { get; set; }
    public bool IsSuccessStatusCode { get; set; }
    public T Data { get; set; }
    public string ErrorContent { get; set; }
}