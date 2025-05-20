using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class QuickCommentsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public QuickCommentsApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public async Task<ApiResponse<List<QuickCommentDto>>> CreateQuickCommentAsync(QuickCommentRequestDto request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/QuickComments", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var quickComments = JsonSerializer.Deserialize<List<QuickCommentDto>>(responseContent);

        return new ApiResponse<List<QuickCommentDto>>
        {
            StatusCode = (int)response.StatusCode,
            Data = quickComments
        };
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
}
