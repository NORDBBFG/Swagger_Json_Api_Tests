using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class QuickCommentApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public QuickCommentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<QuickCommentDto>>> GetQuickCommentsAsync(QuickCommentCategory category)
    {
        var response = await _httpClient.PostAsync($"/api/v1/QuickComments?quickCommentCategory={(int)category}", null);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var quickComments = JsonConvert.DeserializeObject<List<QuickCommentDto>>(content);
            return new ApiResponse<List<QuickCommentDto>>(response.StatusCode, quickComments);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorResult = JsonConvert.DeserializeObject<ContentResult>(errorContent);
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