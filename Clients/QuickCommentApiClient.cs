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
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorResult = JsonSerializer.Deserialize<ContentResult>(content, _jsonOptions);
            return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, null, errorResult);
        }

        return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, null);
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