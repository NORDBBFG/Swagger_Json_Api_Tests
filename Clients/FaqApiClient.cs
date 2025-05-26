using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class FaqApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public FaqApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public async Task<ApiResponse<List<FaqCategoryDto>>> GetFaqCategoriesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/faqs/categories");
        return await ProcessResponseAsync<List<FaqCategoryDto>>(response);
    }

    private async Task<ApiResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<T> { Data = data, StatusCode = (int)response.StatusCode };
        }
        else
        {
            var errorResult = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<T> { ErrorResult = errorResult, StatusCode = (int)response.StatusCode };
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; set; }
    public ContentResult ErrorResult { get; set; }
    public int StatusCode { get; set; }
}