using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class FaqApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    public FaqApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ApiResponse<List<FaqCategoryDto>>> GetFaqCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/faqs/categories");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var categories = JsonSerializer.Deserialize<List<FaqCategoryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<FaqCategoryDto>>(categories, (int)response.StatusCode);
        }
        else
        {
            var errorContent = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<FaqCategoryDto>>(null, errorContent.StatusCode, errorContent.Content);
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; }
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public ApiResponse(T data, int statusCode, string errorMessage = null)
    {
        Data = data;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}