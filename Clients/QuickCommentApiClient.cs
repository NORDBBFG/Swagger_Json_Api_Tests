using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class QuickCommentApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public QuickCommentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<QuickCommentDto>>> GetQuickCommentsByCategoryAsync(QuickCommentCategory category)
    {
        var response = await _httpClient.PostAsync($"/api/v1/QuickComments?quickCommentCategory={category}", null);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var quickComments = JsonSerializer.Deserialize<List<QuickCommentDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, quickComments);
        }

        return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, null, await response.Content.ReadAsStringAsync());
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public string ErrorMessage { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data, string errorMessage = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
    }
}