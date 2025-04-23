using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class QuickCommentsApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public QuickCommentsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<QuickCommentDto>>> GetQuickCommentsByCategoryAsync(QuickCommentCategory category)
    {
        try
        {
            var response = await _httpClient.PostAsync($"/api/v1/QuickComments?quickCommentCategory={category}", null);
            
            if (response.IsSuccessStatusCode)
            {
                var comments = await response.Content.ReadFromJsonAsync<List<QuickCommentDto>>();
                return new ApiResponse<List<QuickCommentDto>>(true, comments, null);
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ContentResult>();
                return new ApiResponse<List<QuickCommentDto>>(false, null, errorContent);
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<QuickCommentDto>>(false, null, new ContentResult { Content = ex.Message, StatusCode = 500 });
        }
    }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public ContentResult Error { get; }

    public ApiResponse(bool isSuccess, T data, ContentResult error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }
}