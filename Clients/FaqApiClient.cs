using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class FaqApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public FaqApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<List<FaqCategoryDto>>> GetFaqCategoriesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/faqs/categories");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var categories = JsonSerializer.Deserialize<List<FaqCategoryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<FaqCategoryDto>>
            {
                Data = categories,
                StatusCode = (int)response.StatusCode
            };
        }
        else
        {
            var errorContent = JsonSerializer.Deserialize<ContentResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new ApiResponse<List<FaqCategoryDto>>
            {
                Error = errorContent,
                StatusCode = (int)response.StatusCode
            };
        }
    }
}

public class ApiResponse<T>
{
    public T Data { get; set; }
    public ContentResult Error { get; set; }
    public int StatusCode { get; set; }
}