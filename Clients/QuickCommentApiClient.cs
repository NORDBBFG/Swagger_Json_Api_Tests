using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class QuickCommentApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public QuickCommentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ApiResponse<List<QuickCommentDto>>> GetQuickCommentsAsync(QuickCommentCategory category)
    {
        var response = await _httpClient.PostAsync($"/api/v1/QuickComments?quickCommentCategory={(int)category}", null);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var quickComments = JsonSerializer.Deserialize<List<QuickCommentDto>>(content, _jsonOptions);
            return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, quickComments);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorResult = JsonSerializer.Deserialize<ContentResult>(errorContent, _jsonOptions);
            return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, null, errorResult);
        }
    }
}

public class ApiResponse<T>
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public ContentResult ErrorResult { get; }

    public ApiResponse(System.Net.HttpStatusCode statusCode, T data, ContentResult errorResult = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorResult = errorResult;
    }
}
