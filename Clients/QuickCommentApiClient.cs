using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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

    public async Task<ApiResponse<List<QuickCommentDto>>> GetQuickCommentsAsync(QuickCommentCategory category)
    {
        var response = await _httpClient.PostAsync($"/api/v1/QuickComments?quickCommentCategory={category}", null);

        if (response.IsSuccessStatusCode)
        {
            var comments = await response.Content.ReadFromJsonAsync<List<QuickCommentDto>>();
            return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, comments);
        }
        else
        {
            var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
            return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, null, errorContent);
        }
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public ContentResult ErrorContent { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data, ContentResult errorContent = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorContent = errorContent;
    }
}